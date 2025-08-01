using Microsoft.EntityFrameworkCore;
using ShopApi.Data;
using ShopApi.Models;
using ShopApi.Repositories.Interfaces;

namespace ShopApi.Repositories
{
    public class ContactUsRepository : IContactUsRepository
    {
        private readonly AppDbContext _context;

        public ContactUsRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ContactUs>> GetAllAsync()
        {
            return await _context.ContactUs.ToListAsync();
        }

        public async Task<ContactUs?> GetByIdAsync(int id)
        {
            return await _context.ContactUs.FindAsync(id);
        }

        public async Task AddAsync(ContactUs contact)
        {
            await _context.ContactUs.AddAsync(contact);
        }

        public void Delete(ContactUs contact)
        {
            _context.ContactUs.Remove(contact);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
