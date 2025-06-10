using AutoMapper;
using EventManagementAPI.Interfaces;
using EventManagementAPI.Models;
using EventManagementAPI.Models.DTOs.Registration;
using EventManagementAPI.Services;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventManagementAPI.Tests
{
    public class RegistrationServiceTests
    {
        private Mock<IRegistrationRepository> _registrationRepoMock;
        private Mock<IEventRepository> _eventRepoMock;
        private Mock<IMapper> _mapperMock;
        private RegistrationService _registrationService;

        private List<Registration> _registrations;
        private List<Event> _events;

        [SetUp]
        public void Setup()
        {
            _registrationRepoMock = new Mock<IRegistrationRepository>();
            _eventRepoMock = new Mock<IEventRepository>();
            _mapperMock = new Mock<IMapper>();

            _registrations = new List<Registration>();
            _events = new List<Event>();

            _registrationService = new RegistrationService(
                _registrationRepoMock.Object,
                _eventRepoMock.Object,
                _mapperMock.Object
            );
        }

        [Test]
        public async Task GetAllRegistrationsForUserAsync_ReturnsMappedList()
        {
            var attendeeId = Guid.NewGuid();
            var registrations = new List<Registration> { new Registration { Id = Guid.NewGuid() } };

            _registrationRepoMock.Setup(r => r.GetByAttendeeIdAsync(attendeeId))
                                 .ReturnsAsync(registrations);

            _mapperMock.Setup(m => m.Map<IEnumerable<RegistrationResponseDto>>(registrations))
                       .Returns(new List<RegistrationResponseDto> { new RegistrationResponseDto { Id = registrations[0].Id } });

            var result = await _registrationService.GetAllRegistrationsForUserAsync(attendeeId);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
        }

        [Test]
        public void RegisterForEventAsync_EventNotFound_ThrowsException()
        {
            _eventRepoMock.Setup(e => e.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Event)null);

            Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _registrationService.RegisterForEventAsync(Guid.NewGuid(), Guid.NewGuid()));
        }

        [Test]
        public async Task RegisterForEventAsync_AlreadyRegistered_ThrowsInvalidOperationException()
        {
            var eventId = Guid.NewGuid();
            var attendeeId = Guid.NewGuid();

            _eventRepoMock.Setup(e => e.GetByIdAsync(eventId)).ReturnsAsync(new Event { Id = eventId });
            _registrationRepoMock.Setup(r => r.GetByEventAndAttendeeIncludingDeletedAsync(eventId, attendeeId))
                                 .ReturnsAsync(new Registration { Id = Guid.NewGuid(), IsDeleted = false });

            var ex = Assert.ThrowsAsync<InvalidOperationException>(() =>
                _registrationService.RegisterForEventAsync(eventId, attendeeId));

            Assert.That(ex.Message, Is.EqualTo("Already registered for this event"));
        }

        [Test]
        public async Task RegisterForEventAsync_RevivesDeletedRegistration()
        {
            var eventId = Guid.NewGuid();
            var attendeeId = Guid.NewGuid();
            var deletedRegistration = new Registration
            {
                Id = Guid.NewGuid(),
                EventId = eventId,
                AttendeeId = attendeeId,
                IsDeleted = true
            };

            _eventRepoMock.Setup(e => e.GetByIdAsync(eventId)).ReturnsAsync(new Event { Id = eventId });
            _registrationRepoMock.Setup(r => r.GetByEventAndAttendeeIncludingDeletedAsync(eventId, attendeeId))
                                 .ReturnsAsync(deletedRegistration);

            _mapperMock.Setup(m => m.Map<RegistrationResponseDto>(deletedRegistration))
                       .Returns(new RegistrationResponseDto { Id = deletedRegistration.Id });

            var result = await _registrationService.RegisterForEventAsync(eventId, attendeeId);

            _registrationRepoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
            Assert.AreEqual(deletedRegistration.Id, result.Id);
        }

        [Test]
        public async Task RegisterForEventAsync_NewRegistration_CreatesSuccessfully()
        {
            var eventId = Guid.NewGuid();
            var attendeeId = Guid.NewGuid();

            _eventRepoMock.Setup(e => e.GetByIdAsync(eventId)).ReturnsAsync(new Event { Id = eventId });
            _registrationRepoMock.Setup(r => r.GetByEventAndAttendeeIncludingDeletedAsync(eventId, attendeeId))
                                 .ReturnsAsync((Registration)null);

            var newReg = new Registration { Id = Guid.NewGuid(), EventId = eventId, AttendeeId = attendeeId };
            _registrationRepoMock.Setup(r => r.AddAsync(It.IsAny<Registration>())).ReturnsAsync(newReg);

            _mapperMock.Setup(m => m.Map<RegistrationResponseDto>(newReg))
                       .Returns(new RegistrationResponseDto { Id = newReg.Id });

            var result = await _registrationService.RegisterForEventAsync(eventId, attendeeId);

            Assert.AreEqual(newReg.Id, result.Id);
        }

        [Test]
        public async Task CancelRegistrationAsync_InvalidUser_ReturnsFalse()
        {
            var regId = Guid.NewGuid();
            var attendeeId = Guid.NewGuid();
            var otherUserId = Guid.NewGuid();

            var reg = new Registration { Id = regId, AttendeeId = otherUserId };
            _registrationRepoMock.Setup(r => r.GetByIdAsync(regId)).ReturnsAsync(reg);

            var result = await _registrationService.CancelRegistrationAsync(regId, attendeeId);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task CancelRegistrationAsync_ValidRequest_DeletesSuccessfully()
        {
            var regId = Guid.NewGuid();
            var attendeeId = Guid.NewGuid();

            var reg = new Registration { Id = regId, AttendeeId = attendeeId };
            _registrationRepoMock.Setup(r => r.GetByIdAsync(regId)).ReturnsAsync(reg);
            _registrationRepoMock.Setup(r => r.DeleteAsync(regId)).ReturnsAsync(true);

            var result = await _registrationService.CancelRegistrationAsync(regId, attendeeId);

            Assert.IsTrue(result);
        }
    }
}
