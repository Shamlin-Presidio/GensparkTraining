using EventManagementAPI.Models;

namespace EventManagementAPI.Interfaces;

public interface IEventRepository
{
    Task<IEnumerable<Event>> GetAllAsync(string? search = null,DateTime? date=null, int page = 1, int pageSize = 10);
    Task<int> CountAsync(string? search = null);
    Task<Event?> GetByIdAsync(Guid id);
    Task<Event> AddAsync(Event ev);
    Task<Event?> UpdateAsync(Event ev);
    Task<bool> DeleteAsync(Guid id);
    Task<IEnumerable<Event>> GetByOrganizerIdAsync(Guid organizerId);
}
