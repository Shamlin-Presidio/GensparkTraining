using EventManagementAPI.Models;
using EventManagementAPI.Models.DTOs.Registration;
using EventManagementAPI.Models.DTOs.User;

namespace EventManagementAPI.Interfaces;

public interface IRegistrationService
{
    Task<IEnumerable<RegistrationResponseDto>> GetAllRegistrationsForUserAsync(Guid attendeeId);
    Task<RegistrationResponseDto> RegisterForEventAsync(Guid eventId, Guid attendeeId);
    Task<bool> CancelRegistrationAsync(Guid registrationId, Guid attendeeId);
    Task<int> GetRegistrationCountAsync(Guid eventId);
    Task<IEnumerable<UserResponseDto>> GetAttendeesForEventAsync(Guid eventId);
    Task<IEnumerable<Registration>> GetRegistrationsForEventAsync(Guid eventId);
}