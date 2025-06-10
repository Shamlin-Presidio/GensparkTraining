using AutoMapper;
using EventManagementAPI.Interfaces;
using EventManagementAPI.Models;
using EventManagementAPI.Models.DTOs.User;
using EventManagementAPI.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

[TestFixture]
public class UserServiceTests
{
    private Mock<IUserRepository> _userRepoMock;
    private Mock<IMapper> _mapperMock;
    private Mock<IWebHostEnvironment> _envMock;
    private UserService _userService;
    private List<User> _users;

    [SetUp]
    public void Setup()
    {
        _userRepoMock = new Mock<IUserRepository>();
        _mapperMock = new Mock<IMapper>();
        _envMock = new Mock<IWebHostEnvironment>();
        _userService = new UserService(_userRepoMock.Object, _mapperMock.Object, _envMock.Object);

        _users = new List<User>
        {
            new User { Id = Guid.NewGuid(), Username = "Nicky", Email = "nicky@example.com", Role = "Attendee", IsDeleted = false, CreatedAt = DateTime.UtcNow },
            new User { Id = Guid.NewGuid(), Username = "Zander", Email = "zander@example.com", Role = "Organizer", IsDeleted = true, CreatedAt = DateTime.UtcNow }
        };
    }

    [Test]
    public async Task GetAllUsersAsync_ReturnsMappedUsers()
    {
        _userRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(_users);
        _mapperMock.Setup(m => m.Map<IEnumerable<UserResponseDto>>(It.IsAny<IEnumerable<User>>()))
                   .Returns(new List<UserResponseDto>());

        var result = await _userService.GetAllUsersAsync();

        Assert.IsInstanceOf<IEnumerable<UserResponseDto>>(result);
        _userRepoMock.Verify(r => r.GetAllAsync(), Times.Once);
    }
    [Test]
    public async Task GetUserByIdAsync_ValidId_ReturnsMappedUser()
    {
        var user = _users[0];
        _userRepoMock.Setup(r => r.GetByIdAsync(user.Id)).ReturnsAsync(user);
        _mapperMock.Setup(m => m.Map<UserResponseDto>(user)).Returns(new UserResponseDto { Id = user.Id });

        var result = await _userService.GetUserByIdAsync(user.Id);

        Assert.NotNull(result);
        Assert.AreEqual(user.Id, result!.Id);
    }

    [Test]
    public async Task GetUserByIdAsync_UserIsDeleted_ReturnsNull()
    {
        var user = _users[1];
        _userRepoMock.Setup(r => r.GetByIdAsync(user.Id)).ReturnsAsync(user);

        var result = await _userService.GetUserByIdAsync(user.Id);

        Assert.IsNull(result);
    }
    [Test]
    public async Task UpdateUserAsync_ValidId_UpdatesUserAndReturnsDto()
    {
        var user = _users[0];
        var dto = new UserUpdateDto { Username = "Updated", Email = "updated@email.com" };

        _userRepoMock.Setup(r => r.GetByIdAsync(user.Id)).ReturnsAsync(user);
        _mapperMock.Setup(m => m.Map(dto, user));
        _userRepoMock.Setup(r => r.UpdateAsync(user)).ReturnsAsync(user);
        _mapperMock.Setup(m => m.Map<UserResponseDto>(user)).Returns(new UserResponseDto { Id = user.Id });

        var result = await _userService.UpdateUserAsync(user.Id, dto);

        Assert.NotNull(result);
        Assert.AreEqual(user.Id, result!.Id);
    }

    [Test]
    public async Task UpdateUserAsync_UserNotFound_ReturnsNull()
    {
        var id = Guid.NewGuid();
        _userRepoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((User?)null);

        var result = await _userService.UpdateUserAsync(id, new UserUpdateDto());

        Assert.IsNull(result);
    }

    [Test]
    public async Task UpdateUserAsync_WithProfilePicture_StoresImageAndUpdatesPath()
    {
        var user = _users[0];
        var mockFile = new Mock<IFormFile>();
        var content = new MemoryStream();
        var writer = new StreamWriter(content);
        writer.Write("fake image");
        writer.Flush();
        content.Position = 0;

        mockFile.Setup(f => f.FileName).Returns("photo.png");
        mockFile.Setup(f => f.OpenReadStream()).Returns(content);
        mockFile.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), default)).Returns((Stream stream, CancellationToken _) => content.CopyToAsync(stream));
        mockFile.Setup(f => f.Length).Returns(content.Length);

        var dto = new UserUpdateDto
        {
            Username = "User",
            Email = "u@example.com",
            ProfilePicture = mockFile.Object
        };

        _envMock.Setup(e => e.ContentRootPath).Returns(Directory.GetCurrentDirectory());
        _userRepoMock.Setup(r => r.GetByIdAsync(user.Id)).ReturnsAsync(user);
        _mapperMock.Setup(m => m.Map(dto, user));
        _userRepoMock.Setup(r => r.UpdateAsync(user)).ReturnsAsync(user);
        _mapperMock.Setup(m => m.Map<UserResponseDto>(user)).Returns(new UserResponseDto { Id = user.Id });

        var result = await _userService.UpdateUserAsync(user.Id, dto);

        Assert.NotNull(result);
        Assert.That(user.ProfilePicturePath, Does.Contain("UploadedFiles/Users"));
    }
    [Test]
    public async Task DeleteUserAsync_ValidId_ReturnsTrue()
    {
        var id = Guid.NewGuid();
        _userRepoMock.Setup(r => r.DeleteAsync(id)).ReturnsAsync(true);

        var result = await _userService.DeleteUserAsync(id);

        Assert.IsTrue(result);
    }

    [Test]
    public async Task DeleteUserAsync_InvalidId_ReturnsFalse()
    {
        var id = Guid.NewGuid();
        _userRepoMock.Setup(r => r.DeleteAsync(id)).ReturnsAsync(false);

        var result = await _userService.DeleteUserAsync(id);

        Assert.IsFalse(result);
    }
}
