using EventManagementAPI.Data;
using EventManagementAPI.Interfaces;
using EventManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EventManagementAPI.Repositories;

public class EventRepository : IEventRepository
{
    private readonly AppDbContext _context;
    public EventRepository(AppDbContext context) => _context = context;

    public async Task<IEnumerable<Event>> GetAllAsync(string? search = null,  DateTime? date =null, int page = 1, int pageSize = 10)
    {
        var query = _context.Events
            .Include(e => e.Organizer)
            .Where(e => !e.IsDeleted);

        // if (!string.IsNullOrWhiteSpace(search))
        // {
        //     query = query.Where(e =>
        //         e.Title.Contains(search) || e.Description.Contains(search) || e.Location.Contains(search));
        // }

        // M O R E   S E A R C H    F I L T E R S 
        if (!string.IsNullOrWhiteSpace(search))
        {
            var lowered = search.ToLower();
            query = query.Where(e =>
                e.Title.ToLower().Contains(lowered) ||
                e.Description.ToLower().Contains(lowered) ||
                e.Location.ToLower().Contains(lowered) 
                // e.StartTime.ToString("dd MMM yyyy").ToLower().Contains(lowered) ||    
                // e.StartTime.ToString("d MMMM yyyy").ToLower().Contains(lowered) ||    
                // e.StartTime.ToString("dd").ToLower().Contains(lowered) ||             
                // e.StartTime.ToString("MMMM").ToLower().Contains(lowered)              
            );
        }
        if (date.HasValue)
        {
            // var selectedDate = date.Value.Date;

            // query = query.Where(e => e.StartTime.Date == selectedDate);
            var selectedDate = DateTime.SpecifyKind(date.Value.Date, DateTimeKind.Utc);
            query = query.Where(e => e.StartTime.Date == selectedDate);
        }

        query = query.OrderByDescending(e => e.CreatedAt);

        return await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }


    public async Task<int> CountAsync(string? search = null)
    {
        var query = _context.Events.Where(e => !e.IsDeleted);

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(e =>
                e.Title.Contains(search) || e.Description.Contains(search));
            // this helps user to search based on their interest
        }

        return await query.CountAsync();
    }


    public async Task<Event?> GetByIdAsync(Guid id)
    {
        return await _context.Events
            .Include(e => e.Organizer)
            .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);
    }

    public async Task<Event> AddAsync(Event ev)
    {
        _context.Events.Add(ev);
        await _context.SaveChangesAsync();
        return ev;
    }

    public async Task<Event?> UpdateAsync(Event ev)
    {
        _context.Events.Update(ev);
        await _context.SaveChangesAsync();
        return ev;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var ev = await GetByIdAsync(id);
        if (ev == null) return false;
        ev.IsDeleted = true;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<Event>> GetByOrganizerIdAsync(Guid organizerId)
    {
        return await _context.Events
            .Where(e => e.OrganizerId == organizerId && !e.IsDeleted)
            .OrderByDescending(e => e.CreatedAt)
            .ToListAsync();
    }
}