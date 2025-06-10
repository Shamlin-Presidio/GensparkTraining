using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EventManagementAPI.Interfaces;
using EventManagementAPI.Models;
using EventManagementAPI.Models.DTOs.Auth;
using EventManagementAPI.Models.DTOs.User;
using EventManagementAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using Moq;
using NUnit.Framework;

namespace EventManagementAPI.Tests.Services;

[TestFixture]
public class AuthServiceTests
{
    private Mock<IUserService> _userServiceMock;
    private Mock<IUserRepository> _userRepositoryMock;
    private Mock<IJwtService> _jwtServiceMock;
    private Mock<IWebHostEnvironment> _envMock;

    private AuthService _authService;
    private List<User> _usersInMemory;

    [SetUp]
    public void Setup()
    {
        _userServiceMock = new Mock<IUserService>();

        _usersInMemory = new List<User>();
        _userRepositoryMock = new Mock<IUserRepository>();

        // Setup AddAsync to add to in-memory list
        _userRepositoryMock.Setup(r => r.AddAsync(It.IsAny<User>()))
            .Returns<User>(user =>
            {
                _usersInMemory.Add(user);
                return Task.FromResult(user);
            });

        // Setup GetAllAsync to return in-memory users
        _userRepositoryMock.Setup(r => r.GetAllAsync())
            .ReturnsAsync(() => _usersInMemory.ToList());

        _jwtServiceMock = new Mock<IJwtService>();
        _jwtServiceMock.Setup(j => j.GenerateAccessToken(It.IsAny<User>()))
            .Returns("access_token_mock");
        _jwtServiceMock.Setup(j => j.GenerateRefreshToken(It.IsAny<User>()))
            .Returns("refresh_token_mock");

        _envMock = new Mock<IWebHostEnvironment>();
        _envMock.Setup(e => e.ContentRootPath).Returns(Directory.GetCurrentDirectory());

        _authService = new AuthService(
            _userServiceMock.Object,
            _userRepositoryMock.Object,
            _jwtServiceMock.Object,
            _envMock.Object);
    }

    [Test]
    public async Task SignUpAsync_Should_Create_User_And_Return_Tokens()
    {
        var dto = new UserCreateDto
        {
            Username = "testuser",
            Password = "password123",
            Email = "test@example.com",
            Role = "User"
        };

        var (token, refreshToken, userDto) = await _authService.SignUpAsync(dto);

        Assert.AreEqual("access_token_mock", token);
        Assert.AreEqual("refresh_token_mock", refreshToken);
        Assert.NotNull(userDto);
        Assert.AreEqual(dto.Username, userDto.Username);
        Assert.AreEqual(dto.Email, userDto.Email);
        Assert.AreEqual(dto.Role, userDto.Role);
        Assert.IsTrue(_usersInMemory.Any(u => u.Username == dto.Username));
    }

    [Test]
    public async Task LoginAsync_Should_Return_Tokens_When_Valid_Credentials()
    {
        // Arrange: Add user to in-memory list with hashed password
        var password = "password123";
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = "loginuser",
            PasswordHash = hashedPassword,
            Email = "login@example.com",
            Role = "User",
            IsDeleted = false,
            CreatedAt = DateTime.UtcNow
        };
        _usersInMemory.Add(user);

        var loginDto = new LoginRequestDto
        {
            Username = "loginuser",
            Password = password
        };

        // Act
        var (token, refreshToken, userDto) = await _authService.LoginAsync(loginDto);

        // Assert
        Assert.AreEqual("access_token_mock", token);
        Assert.AreEqual("refresh_token_mock", refreshToken);
        Assert.NotNull(userDto);
        Assert.AreEqual(user.Username, userDto.Username);
        Assert.AreEqual(user.Email, userDto.Email);
        Assert.AreEqual(user.Role, userDto.Role);
    }

    [Test]
    public void LoginAsync_Should_Throw_When_Invalid_Username()
    {
        var loginDto = new LoginRequestDto
        {
            Username = "nonexistentuser",
            Password = "somepassword"
        };

        Assert.ThrowsAsync<UnauthorizedAccessException>(() => _authService.LoginAsync(loginDto));
    }

    [Test]
    public void LoginAsync_Should_Throw_When_Invalid_Password()
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = "validuser",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("correctpassword"),
            Email = "valid@example.com",
            Role = "User",
            IsDeleted = false,
            CreatedAt = DateTime.UtcNow
        };
        _usersInMemory.Add(user);

        var loginDto = new LoginRequestDto
        {
            Username = "validuser",
            Password = "wrongpassword"
        };

        Assert.ThrowsAsync<UnauthorizedAccessException>(() => _authService.LoginAsync(loginDto));
    }
    [Test]
    public async Task SignUpAsync_WithProfilePicture_SavesUserAndReturnsTokens()
    {
        // Arrange
        var dto = new UserCreateDto
        {
            Username = "testuser",
            Email = "test@example.com",
            Password = "Password123",
            Role = "Attendee"
        };

        var mockFile = new Mock<IFormFile>();
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        writer.Write("fake image data");
        writer.Flush();
        stream.Position = 0;

        mockFile.Setup(f => f.FileName).Returns("profile.png");
        mockFile.Setup(f => f.Length).Returns(stream.Length);
        mockFile.Setup(f => f.OpenReadStream()).Returns(stream);
        mockFile.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), default)).Returns<Stream, System.Threading.CancellationToken>((target, _) => stream.CopyToAsync(target));

        _envMock.Setup(e => e.ContentRootPath).Returns(Directory.GetCurrentDirectory());

        User? savedUser = null;
        _userRepositoryMock.Setup(r => r.AddAsync(It.IsAny<User>()))
            .Returns<User>(user =>
            {
                savedUser = user;
                return Task.FromResult(user);
            });

        _jwtServiceMock.Setup(j => j.GenerateAccessToken(It.IsAny<User>())).Returns("mocked-access-token");
        _jwtServiceMock.Setup(j => j.GenerateRefreshToken(It.IsAny<User>())).Returns("mocked-refresh-token");

        // Act
        var (token, refreshToken, userDto) = await _authService.SignUpAsync(dto, mockFile.Object);

        // Assert
        Assert.AreEqual("mocked-access-token", token);
        Assert.AreEqual("mocked-refresh-token", refreshToken);
        Assert.AreEqual(dto.Username, userDto.Username);
        Assert.IsNotNull(savedUser);
        Assert.That(savedUser!.ProfilePicturePath, Does.Contain("UploadedFiles/Users"));
    }
}
