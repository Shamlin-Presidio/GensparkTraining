using EventManagementAPI.Models.DTOs.Registration;

namespace EventManagementAPI.Interfaces;

public interface IRegistrationService
{
    Task<IEnumerable<RegistrationResponseDto>> GetAllRegistrationsForUserAsync(Guid attendeeId);
    Task<RegistrationResponseDto> RegisterForEventAsync(Guid eventId, Guid attendeeId);
    Task<bool> CancelRegistrationAsync(Guid registrationId, Guid attendeeId);
    Task<int> GetRegistrationCountAsync(Guid eventId);

}