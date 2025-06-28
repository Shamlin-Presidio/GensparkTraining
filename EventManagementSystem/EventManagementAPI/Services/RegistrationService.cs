using AutoMapper;
using EventManagementAPI.Interfaces;
using EventManagementAPI.Models;
using EventManagementAPI.Models.DTOs.Registration;

namespace EventManagementAPI.Services
{
    public class RegistrationService : IRegistrationService
    {
        private readonly IRegistrationRepository _registrationRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IMapper _mapper;

        public RegistrationService(
            IRegistrationRepository registrationRepository,
            IEventRepository eventRepository,
            IMapper mapper)
        {
            _registrationRepository = registrationRepository;
            _eventRepository = eventRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RegistrationResponseDto>> GetAllRegistrationsForUserAsync(Guid attendeeId)
        {
            var registrations = await _registrationRepository.GetByAttendeeIdAsync(attendeeId);
            return _mapper.Map<IEnumerable<RegistrationResponseDto>>(registrations);
        }

        // public async Task<RegistrationResponseDto> RegisterForEventAsync(Guid eventId, Guid attendeeId)
        // {

        //     var evnt = await _eventRepository.GetByIdAsync(eventId);
        //     if (evnt == null || evnt.IsDeleted)
        //         throw new KeyNotFoundException("Event not found");


        //     var existing = await _registrationRepository.GetByEventAndAttendeeAsync(eventId, attendeeId);
        //     if (existing != null)
        //         throw new InvalidOperationException("Already registered for this event");

        //     var registration = new Registration
        //     {
        //         Id = Guid.NewGuid(),
        //         EventId = eventId,
        //         AttendeeId = attendeeId,
        //         RegisteredAt = DateTime.UtcNow
        //     };

        //     var created = await _registrationRepository.AddAsync(registration);
        //     return _mapper.Map<RegistrationResponseDto>(created);
        // }

        public async Task<RegistrationResponseDto> RegisterForEventAsync(Guid eventId, Guid attendeeId)
        {
            var evnt = await _eventRepository.GetByIdAsync(eventId);
            if (evnt == null || evnt.IsDeleted)
                throw new KeyNotFoundException("Event not found");
            
            // Prevent registration on or after event date
            if (DateTime.UtcNow.Date >= evnt.StartTime.Date)
                throw new InvalidOperationException("Cannot register for the event on or after the event date.");

            // fetch existing registration (active or soft‐deleted) (if any)
            var existing = await _registrationRepository.GetByEventAndAttendeeIncludingDeletedAsync(eventId, attendeeId);

            if (existing != null)
            {
                if (!existing.IsDeleted)
                {
                    throw new InvalidOperationException("Already registered for this event");
                }
                // existing.IsDeleted == true → reactivate
                existing.IsDeleted = false;
                existing.RegisteredAt = DateTime.UtcNow;
                await _registrationRepository.SaveChangesAsync();

                return _mapper.Map<RegistrationResponseDto>(existing);
            }

            // No existing registration, create new 
            var registration = new Registration
            {
                Id = Guid.NewGuid(),
                EventId = eventId,
                AttendeeId = attendeeId,
                RegisteredAt = DateTime.UtcNow,
                IsDeleted = false
            };

            var created = await _registrationRepository.AddAsync(registration);
            return _mapper.Map<RegistrationResponseDto>(created);
        }

        public async Task<bool> CancelRegistrationAsync(Guid registrationId, Guid attendeeId)
        {
            var reg = await _registrationRepository.GetByIdAsync(registrationId);
            if (reg == null || reg.AttendeeId != attendeeId)
                return false;

            return await _registrationRepository.DeleteAsync(registrationId);
        }
        public async Task<int> GetRegistrationCountAsync(Guid eventId)
        {
            return await _registrationRepository.GetRegistrationCountForEventAsync(eventId);
        }
    }
}
