using FirstApi.Contexts;
using FirstApi.Models;
using FirstApi.Services;
using FirstApi.Interfaces;
using FirstApi.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using FirstApi.Repositories;

namespace FirstApi.Test;

public class AuthenticationServiceTests
{
    private ClinicContext _context;
    private AuthenticationService _service;
    private Mock<IEncryptionService> _encryptionMock;
    private Mock<ITokenService> _tokenMock;
    private Mock<ILogger<AuthenticationService>> _loggerMock;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ClinicContext>()
            .UseInMemoryDatabase("AuthDb")
            .Options;
        _context = new ClinicContext(options);

        var userRepo = new UserRepository(_context);
        _encryptionMock = new Mock<IEncryptionService>();
        _tokenMock = new Mock<ITokenService>();
        _loggerMock = new Mock<ILogger<AuthenticationService>>();
        _service = new AuthenticationService(_tokenMock.Object, _encryptionMock.Object, userRepo, _loggerMock.Object);
    }

    [Test]
    public async Task Login_ValidUser_ReturnsToken()
    {
        var password = "secret";
        var key = Guid.NewGuid().ToByteArray();
        var encrypted = System.Text.Encoding.UTF8.GetBytes(password);

        var user = new User
        {
            Username = "testuser",
            Password = encrypted,
            HashKey = key,
            Role = "Patient"
        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        _encryptionMock.Setup(e => e.EncryptData(It.IsAny<EncryptModel>()))
                       .ReturnsAsync(new EncryptModel { EncryptedData = encrypted });
        _tokenMock.Setup(t => t.GenerateToken(user, null)).ReturnsAsync("fake-token");

        var request = new UserLoginRequest { Username = "testuser", Password = password };

        var result = await _service.Login(request);

        Assert.That(result.Token, Is.EqualTo("fake-token"));
        Assert.That(result.Username, Is.EqualTo("testuser"));
    }

    [TearDown]
    public void TearDown() => _context.Dispose();
}
