// using NUnit.Framework;
// using Moq;
// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.IO;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.SignalR;
// using Microsoft.AspNetCore.Http;
// using Microsoft.Extensions.Hosting;
// using Microsoft.AspNetCore.Hosting;
// using AutoMapper;
// using EventManagementAPI.Services;
// using EventManagementAPI.Interfaces;
// using EventManagementAPI.Hubs;
// using EventManagementAPI.Models;
// using EventManagementAPI.Models.DTOs.Event;

// namespace EventManagementAPI.Tests
// {
//     public class EventServiceTests
//     {
//         private Mock<IEventRepository> _eventRepoMock = null!;
//         private Mock<IMapper> _mapperMock = null!;
//         private Mock<IWebHostEnvironment> _envMock = null!;
//         private Mock<IHubContext<EventHub>> _hubContextMock = null!;
//         private EventService _eventService = null!;

//         private List<Event> _eventsInMemory = null!;

//         [SetUp]
//         public void Setup()
//         {
//             _eventRepoMock = new Mock<IEventRepository>();
//             _mapperMock = new Mock<IMapper>();
//             _envMock = new Mock<IWebHostEnvironment>();
//             _hubContextMock = new Mock<IHubContext<EventHub>>();

//             _envMock.Setup(e => e.ContentRootPath).Returns(Directory.GetCurrentDirectory());

//             _eventsInMemory = new List<Event>();

//             _eventRepoMock.Setup(r => r.GetAllAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
//                           .ReturnsAsync((string? s, int p, int ps) => _eventsInMemory);

//             _eventRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
//                           .ReturnsAsync((Guid id) => _eventsInMemory.FirstOrDefault(e => e.Id == id));

//             _eventRepoMock.Setup(r => r.AddAsync(It.IsAny<Event>()))
//                           .ReturnsAsync((Event e) =>
//                           {
//                               _eventsInMemory.Add(e);
//                               return e;
//                           });

//             _eventRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Event>()))
//                           .Callback<Event>(e =>
//                           {
//                               var index = _eventsInMemory.FindIndex(x => x.Id == e.Id);
//                               if (index != -1) _eventsInMemory[index] = e;
//                           })
//                           .ReturnsAsync((Event e) =>
//                            {
//                                _eventsInMemory.Add(e);
//                                return e;
//                            });

//             _eventService = new EventService(
//                 _eventRepoMock.Object,
//                 _mapperMock.Object,
//                 _envMock.Object,
//                 _hubContextMock.Object
//             );
//         }

//         [Test]
//         public async Task GetAllEventsAsync_ReturnsMappedEvents()
//         {
//             _eventsInMemory.Add(new Event { Id = Guid.NewGuid(), Title = "Test Event" });

//             _mapperMock.Setup(m => m.Map<IEnumerable<EventResponseDto>>(It.IsAny<IEnumerable<Event>>()))
//                        .Returns(new List<EventResponseDto> { new EventResponseDto { Title = "Test Event" } });

//             var result = await _eventService.GetAllEventsAsync();

//             Assert.That(result, Is.Not.Null);
//             Assert.That(result.Count(), Is.EqualTo(1));
//             Assert.That(result.First().Title, Is.EqualTo("Test Event"));
//         }

//         [Test]
//         public async Task GetEventByIdAsync_ReturnsMappedEvent()
//         {
//             var eventId = Guid.NewGuid();
//             _eventsInMemory.Add(new Event { Id = eventId, Title = "One Event" });

//             _mapperMock.Setup(m => m.Map<EventResponseDto>(It.IsAny<Event>()))
//                        .Returns(new EventResponseDto { Title = "One Event" });

//             var result = await _eventService.GetEventByIdAsync(eventId);

//             Assert.That(result, Is.Not.Null);
//             Assert.That(result!.Title, Is.EqualTo("One Event"));
//         }

//         [Test]
//         public async Task CreateEventAsync_AddsEventToRepo()
//         {
//             var dto = new EventCreateDto
//             {
//                 Title = "New Event",
//                 Location = "Here",
//                 StartTime = DateTime.Now,
//                 EndTime = DateTime.Now.AddHours(2)
//             };

//             _mapperMock.Setup(m => m.Map<Event>(dto)).Returns(new Event { Title = dto.Title });

//             _mapperMock.Setup(m => m.Map<EventResponseDto>(It.IsAny<Event>()))
//                        .Returns(new EventResponseDto { Title = dto.Title });

//             _eventRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
//                           .ReturnsAsync((Guid id) => _eventsInMemory.FirstOrDefault(e => e.Id == id));

//             var result = await _eventService.CreateEventAsync(dto, Guid.NewGuid(), null);

//             Assert.That(result.Title, Is.EqualTo("New Event"));
//         }

//         [Test]
//         public async Task UpdateEventAsync_ValidOrganizer_UpdatesEvent()
//         {
//             var organizerId = Guid.NewGuid();
//             var eventId = Guid.NewGuid();

//             _eventsInMemory.Add(new Event { Id = eventId, Title = "Before Update", OrganizerId = organizerId });

//             var dto = new EventUpdateDto { Title = "Updated Event" };

//             _mapperMock.Setup(m => m.Map(dto, It.IsAny<Event>()))
//                        .Callback((EventUpdateDto d, Event e) => e.Title = d.Title);

//             _mapperMock.Setup(m => m.Map<EventResponseDto>(It.IsAny<Event>()))
//                        .Returns((Event e) => new EventResponseDto { Title = e.Title });

//             var result = await _eventService.UpdateEventAsync(eventId, dto, organizerId);

//             Assert.That(result, Is.Not.Null);
//             Assert.That(result!.Title, Is.EqualTo("Updated Event"));
//         }

//         [Test]
//         public async Task DeleteEventAsync_ValidRequest_SoftDeletesEvent()
//         {
//             var eventId = Guid.NewGuid();
//             var organizerId = Guid.NewGuid();
//             _eventsInMemory.Add(new Event { Id = eventId, Title = "To Delete", OrganizerId = organizerId });

//             var result = await _eventService.DeleteEventAsync(eventId, organizerId);

//             Assert.That(result, Is.True);
//             Assert.That(_eventsInMemory.First().IsDeleted, Is.True);
//         }
//     }
// }


using NUnit.Framework;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using AutoMapper;
using EventManagementAPI.Services;
using EventManagementAPI.Interfaces;
using EventManagementAPI.Hubs;
using EventManagementAPI.Models;
using EventManagementAPI.Models.DTOs.Event;

namespace EventManagementAPI.Tests
{
    public class EventServiceTests
    {
        private Mock<IEventRepository> _eventRepoMock = null!;
        private Mock<IMapper> _mapperMock = null!;
        private Mock<IWebHostEnvironment> _envMock = null!;
        private Mock<IHubContext<EventHub>> _hubContextMock = null!;
        private EventService _eventService = null!;

        private List<Event> _eventsInMemory = null!;

        [SetUp]
        public void Setup()
        {
            _eventRepoMock = new Mock<IEventRepository>();
            _mapperMock = new Mock<IMapper>();
            _envMock = new Mock<IWebHostEnvironment>();
            _hubContextMock = new Mock<IHubContext<EventHub>>();

            var mockClients = new Mock<IHubClients>();
            var mockClientProxy = new Mock<IClientProxy>();

            mockClients.Setup(clients => clients.All).Returns(mockClientProxy.Object);
            mockClientProxy
                .Setup(x => x.SendCoreAsync(It.IsAny<string>(), It.IsAny<object[]>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);


            _hubContextMock.Setup(context => context.Clients).Returns(mockClients.Object);

            _envMock.Setup(e => e.ContentRootPath).Returns(Directory.GetCurrentDirectory());

            _eventsInMemory = new List<Event>();

            _eventRepoMock.Setup(r => r.GetAllAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                          .ReturnsAsync((string? s, int p, int ps) => _eventsInMemory);

            _eventRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                          .ReturnsAsync((Guid id) => _eventsInMemory.FirstOrDefault(e => e.Id == id));

            _eventRepoMock.Setup(r => r.AddAsync(It.IsAny<Event>()))
                          .ReturnsAsync((Event e) =>
                          {
                              _eventsInMemory.Add(e);
                              return e;
                          });

            _eventRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Event>()))
                          .Callback<Event>(e =>
                          {
                              var index = _eventsInMemory.FindIndex(x => x.Id == e.Id);
                              if (index != -1) _eventsInMemory[index] = e;
                          })
                          .ReturnsAsync((Event e) =>
                           {
                               _eventsInMemory.Add(e);
                               return e;
                           });

            _eventService = new EventService(
                _eventRepoMock.Object,
                _mapperMock.Object,
                _envMock.Object,
                _hubContextMock.Object
            );
        }

        [Test]
        public async Task GetAllEventsAsync_ReturnsMappedEvents()
        {
            _eventsInMemory.Add(new Event { Id = Guid.NewGuid(), Title = "Test Event" });

            _mapperMock.Setup(m => m.Map<IEnumerable<EventResponseDto>>(It.IsAny<IEnumerable<Event>>()))
                       .Returns(new List<EventResponseDto> { new EventResponseDto { Title = "Test Event" } });

            var result = await _eventService.GetAllEventsAsync();

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First().Title, Is.EqualTo("Test Event"));
        }

        [Test]
        public async Task GetEventByIdAsync_ReturnsMappedEvent()
        {
            var eventId = Guid.NewGuid();
            _eventsInMemory.Add(new Event { Id = eventId, Title = "One Event" });

            _mapperMock.Setup(m => m.Map<EventResponseDto>(It.IsAny<Event>()))
                       .Returns(new EventResponseDto { Title = "One Event" });

            var result = await _eventService.GetEventByIdAsync(eventId);

            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Title, Is.EqualTo("One Event"));
        }

        [Test]
        public async Task CreateEventAsync_AddsEventToRepo()
        {
            var dto = new EventCreateDto
            {
                Title = "New Event",
                Location = "Here",
                StartTime = DateTime.Now,
                EndTime = DateTime.Now.AddHours(2)
            };

            _mapperMock.Setup(m => m.Map<Event>(dto)).Returns(new Event
            {
                Title = dto.Title,
                Location = dto.Location,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime
            });

            _mapperMock.Setup(m => m.Map<EventResponseDto>(It.IsAny<Event>()))
                       .Returns(new EventResponseDto { Title = dto.Title });

            var result = await _eventService.CreateEventAsync(dto, Guid.NewGuid(), null);

            Assert.That(result.Title, Is.EqualTo("New Event"));
            Assert.That(_eventsInMemory.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task CreateEventAsync_AddsEventToRepo_WithImage()
        {
            // Arrange
            var dto = new EventCreateDto
            {
                Title = "New Event",
                Location = "Here",
                StartTime = DateTime.Now,
                EndTime = DateTime.Now.AddHours(2)
            };

            var imageContent = new MemoryStream();
            var writer = new StreamWriter(imageContent);
            writer.Write("fake image content");
            writer.Flush();
            imageContent.Position = 0;

            var mockImage = new Mock<IFormFile>();
            mockImage.Setup(f => f.FileName).Returns("event.jpg");
            mockImage.Setup(f => f.OpenReadStream()).Returns(imageContent);
            mockImage.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), default))
                     .Returns((Stream stream, CancellationToken _) => imageContent.CopyToAsync(stream));
            mockImage.Setup(f => f.Length).Returns(imageContent.Length);

            var mappedEvent = new Event
            {
                Id = Guid.NewGuid(),
                Title = dto.Title,
                Location = dto.Location,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime
            };

            _mapperMock.Setup(m => m.Map<Event>(dto)).Returns(mappedEvent);
            _mapperMock.Setup(m => m.Map<EventResponseDto>(It.IsAny<Event>()))
                       .Returns(new EventResponseDto { Title = dto.Title });

            _envMock.Setup(e => e.ContentRootPath).Returns(Directory.GetCurrentDirectory());

            // Act
            var result = await _eventService.CreateEventAsync(dto, Guid.NewGuid(), mockImage.Object);

            // Assert
            Assert.That(result.Title, Is.EqualTo("New Event"));
            Assert.That(_eventsInMemory.Count, Is.EqualTo(1));
            Assert.That(_eventsInMemory[0].ImagePath, Does.Contain("UploadedFiles/Events"));
        }



        [Test]
        public async Task UpdateEventAsync_ValidOrganizer_UpdatesEvent()
        {
            var organizerId = Guid.NewGuid();
            var eventId = Guid.NewGuid();

            _eventsInMemory.Add(new Event { Id = eventId, Title = "Before Update", OrganizerId = organizerId });

            var dto = new EventUpdateDto { Title = "Updated Event" };

            _mapperMock.Setup(m => m.Map(dto, It.IsAny<Event>()))
                       .Callback((EventUpdateDto d, Event e) => e.Title = d.Title);

            _mapperMock.Setup(m => m.Map<EventResponseDto>(It.IsAny<Event>()))
                       .Returns((Event e) => new EventResponseDto { Title = e.Title });

            var result = await _eventService.UpdateEventAsync(eventId, dto, organizerId);

            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Title, Is.EqualTo("Updated Event"));
        }

        [Test]
        public async Task DeleteEventAsync_ValidRequest_SoftDeletesEvent()
        {
            var eventId = Guid.NewGuid();
            var organizerId = Guid.NewGuid();
            _eventsInMemory.Add(new Event { Id = eventId, Title = "To Delete", OrganizerId = organizerId });

            var result = await _eventService.DeleteEventAsync(eventId, organizerId);

            Assert.That(result, Is.True);
            Assert.That(_eventsInMemory.First().IsDeleted, Is.True);
        }
    }
}
