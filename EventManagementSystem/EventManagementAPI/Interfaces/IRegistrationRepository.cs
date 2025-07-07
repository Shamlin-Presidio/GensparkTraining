using EventManagementAPI.Models;

namespace EventManagementAPI.Interfaces;

public interface IRegistrationRepository
{
    Task<IEnumerable<Registration>> GetByAttendeeIdAsync(Guid attendeeId);
    Task<Registration?> GetByIdAsync(Guid registrationId);
    Task<Registration?> GetByEventAndAttendeeAsync(Guid eventId, Guid attendeeId);
    Task<Registration> AddAsync(Registration registration);
    Task<bool> DeleteAsync(Guid id);
    Task SaveChangesAsync();
    Task<Registration?> GetByEventAndAttendeeIncludingDeletedAsync(Guid eventId, Guid attendeeId);
    Task<int> GetRegistrationCountForEventAsync(Guid eventId);
    Task<IEnumerable<User>> GetAttendeesByEventIdAsync(Guid eventId);


}
