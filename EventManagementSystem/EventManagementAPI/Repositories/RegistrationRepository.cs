using EventManagementAPI.Data;
using EventManagementAPI.Interfaces;
using EventManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EventManagementAPI.Repositories;

public class RegistrationRepository : IRegistrationRepository
{
    private readonly AppDbContext _context;
    public RegistrationRepository(AppDbContext context) => _context = context;

    public async Task<IEnumerable<Registration>> GetByAttendeeIdAsync(Guid attendeeId)
    {
        return await _context.Registrations
            .Include(r => r.Event)
            .Where(r => r.AttendeeId == attendeeId && !r.IsDeleted)
            .ToListAsync();
    }

    public async Task<Registration?> GetByIdAsync(Guid id)
    {
        return await _context.Registrations
            .Include(r => r.Event)
            .FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted);
    }

    public async Task<Registration?> GetByEventAndAttendeeAsync(Guid eventId, Guid attendeeId)
    {
        return await _context.Registrations
            .FirstOrDefaultAsync(r =>
                r.EventId == eventId &&
                r.AttendeeId == attendeeId &&
                !r.IsDeleted);
    }

    public async Task<Registration> AddAsync(Registration registration)
    {
        _context.Registrations.Add(registration);
        await _context.SaveChangesAsync();
        return registration;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var reg = await GetByIdAsync(id);
        if (reg == null) return false;
        reg.IsDeleted = true;
        await _context.SaveChangesAsync();
        return true;
    }
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<Registration?> GetByEventAndAttendeeIncludingDeletedAsync(Guid eventId, Guid attendeeId)
    {
        // N O   checks for IsDeleted!
        return await _context.Registrations
            .FirstOrDefaultAsync(r =>
                r.EventId == eventId &&
                r.AttendeeId == attendeeId);
    }
    public async Task<int> GetRegistrationCountForEventAsync(Guid eventId)
    {
        return await _context.Registrations
            .Where(r => r.EventId == eventId && !r.IsDeleted)
            .CountAsync();
    }

    public async Task<IEnumerable<User>> GetAttendeesByEventIdAsync(Guid eventId)
    {
        return await _context.Registrations
            .Where(r => r.EventId == eventId && !r.IsDeleted)
            .Select(r => r.Attendee)
            .ToListAsync();
    }


}
