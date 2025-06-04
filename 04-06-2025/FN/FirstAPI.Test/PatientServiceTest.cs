using FirstApi.Contexts;
using FirstApi.Interfaces;
using FirstApi.Models;
using FirstApi.Models.DTOs.Patients;
using FirstApi.Repositories;
using FirstApi.Services;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Moq;

namespace FirstApi.Test;

public class PatientServiceTests
{
    private ClinicContext _context;
    private PatientService _service;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ClinicContext>()
                        .UseInMemoryDatabase("PatientDb")
                        .Options;
        _context = new ClinicContext(options);

        var patientRepo = new PatientRepository(_context);
        var userRepo = new UserRepository(_context);

        var encryptionMock = new Mock<IEncryptionService>();
        encryptionMock.Setup(e => e.EncryptData(It.IsAny<EncryptModel>()))
                      .ReturnsAsync(new EncryptModel
                      {
                          EncryptedData = System.Text.Encoding.UTF8.GetBytes("test123"),
                          HashKey = Guid.NewGuid().ToByteArray()
                      });

        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<PatientAddRequestDto, User>();
            cfg.CreateMap<PatientAddRequestDto, Patient>();
        });

        _service = new PatientService(patientRepo, userRepo, encryptionMock.Object, config.CreateMapper());
    }

    [Test]
    public async Task AddPatient_ReturnsPatient()
    {
        var dto = new PatientAddRequestDto
        {
            Name = "Johni boy",
            Age = 30,
            Email = "johni@boy.com",
            Address = "summa summa",
            Password = "secretisasecret"
        };

        var result = await _service.AddPatient(dto);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Name, Is.EqualTo("Johni boy"));
    }

    [TearDown]
    public void TearDown() => _context.Dispose();
}
