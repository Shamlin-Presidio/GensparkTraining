using System.IO;
using AutoMapper;
using EventManagementAPI.Hubs;
using EventManagementAPI.Interfaces;
using EventManagementAPI.Models;
using EventManagementAPI.Models.DTOs.Event;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;

public class EventService : IEventService
{
    private readonly IEventRepository _eventRepository;
    private readonly IMapper _mapper;
    private readonly IWebHostEnvironment _env;
    private readonly IHubContext<EventHub> _hubContext;

    public EventService(IEventRepository eventRepository, IMapper mapper, IWebHostEnvironment env, IHubContext<EventHub> hubContext)
    {
        _eventRepository = eventRepository;
        _mapper = mapper;
        _env = env;
        _hubContext = hubContext;
    }

    public async Task<IEnumerable<EventResponseDto>> GetAllEventsAsync(string? search = null, int page = 1, int pageSize = 10)
    {
        var events = await _eventRepository.GetAllAsync(search, page, pageSize);
        return _mapper.Map<IEnumerable<EventResponseDto>>(events);
    }

    public async Task<EventResponseDto?> GetEventByIdAsync(Guid id)
    {
        var evnt = await _eventRepository.GetByIdAsync(id);
        if (evnt == null)
            return null;

        return _mapper.Map<EventResponseDto>(evnt);
    }

    public async Task<EventResponseDto> CreateEventAsync(EventCreateDto dto, Guid organizerId, IFormFile? image = null)
    {
        var evnt = _mapper.Map<Event>(dto);
        evnt.Id = Guid.NewGuid();
        evnt.OrganizerId = organizerId;

        if (image != null)
        {
            var folder = Path.Combine("UploadedFiles", "Events");
            var extension = Path.GetExtension(image.FileName);
            var fileName = $"{evnt.Id}{extension}";
            var folderPath = Path.Combine(_env.ContentRootPath, folder);
            Directory.CreateDirectory(folderPath);
            var filePath = Path.Combine(folderPath, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await image.CopyToAsync(stream);

            evnt.ImagePath = Path.Combine(folder, fileName).Replace("\\", "/");
        }

        var createdEvent = await _eventRepository.AddAsync(evnt);

        await _hubContext.Clients.All.SendAsync("NewEventCreated", evnt.Title);
        
        return _mapper.Map<EventResponseDto>(createdEvent);
    }

    public async Task<EventResponseDto?> UpdateEventAsync(Guid id, EventUpdateDto dto, Guid organizerId, IFormFile? image = null)
    {
        var evnt = await _eventRepository.GetByIdAsync(id);
        if (evnt == null || evnt.IsDeleted || evnt.OrganizerId != organizerId)
            return null;

        _mapper.Map(dto, evnt);

        if (image != null)
        {
            var folder = Path.Combine("UploadedFiles", "Events");
            var extension = Path.GetExtension(image.FileName);
            var fileName = $"{evnt.Id}{extension}";
            var folderPath = Path.Combine(_env.ContentRootPath, folder);
            Directory.CreateDirectory(folderPath);
            var filePath = Path.Combine(folderPath, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await image.CopyToAsync(stream);

            evnt.ImagePath = Path.Combine(folder, fileName).Replace("\\", "/");
        }

        await _eventRepository.UpdateAsync(evnt);
        return _mapper.Map<EventResponseDto>(evnt);
    }

    public async Task<bool> DeleteEventAsync(Guid id, Guid organizerId)
    {
        var evnt = await _eventRepository.GetByIdAsync(id);
        if (evnt == null || evnt.IsDeleted || evnt.OrganizerId != organizerId)
            return false;

        evnt.IsDeleted = true;
        await _eventRepository.UpdateAsync(evnt);
        return true;
    }
}
