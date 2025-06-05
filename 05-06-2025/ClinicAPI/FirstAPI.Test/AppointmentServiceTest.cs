using FirstApi.Contexts;
using FirstApi.Models;
using FirstApi.Models.DTOs;
using FirstApi.Repositories;
using FirstApi.Services;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace FirstApi.Test;

public class AppointmentServiceTests
{
    private ClinicContext _context;
    private AppointmentService _service;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ClinicContext>()
                        .UseInMemoryDatabase("AppointmentDb")
                        .Options;
        _context = new ClinicContext(options);

        var repo = new AppointmentRepository(_context);

        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<AppointmentAddRequestDto, Appointment>();
        });

        _service = new AppointmentService(repo, config.CreateMapper());
    }

    [Test]
    public async Task AddAppointment_ReturnsAppointment()
    {
        var dto = new AppointmentAddRequestDto
        {
            DoctorId = 1,
            PatientId = 1,
            AppointmnetDateTime = DateTime.Now,
            Status = "Pending"
        };

        var result = await _service.AddAppointment(dto);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.AppointmentNumber, Is.Not.Empty);
    }

    [Test]
    public async Task DeleteAppointment_ReturnsTrue()
    {
        var appointment = new Appointment
        {
            AppointmentNumber = "123",
            DoctorId = 1,
            PatientId = 1,
            AppointmentDateTime = DateTime.Now,
            Status = "Scheduled"
        };
        _context.Appointments.Add(appointment);
        await _context.SaveChangesAsync();

        var repo = new AppointmentRepository(_context);
        var result = await repo.Delete("123");

        Assert.That(result, Is.Not.Null);
    }

    [TearDown]
    public void TearDown() => _context.Dispose();
}
