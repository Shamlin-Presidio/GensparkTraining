using Microsoft.EntityFrameworkCore;
using docuNotify.Models;

namespace docuNotify.Contexts
{
    public class DocuNotifyContext : DbContext
    {
        public DocuNotifyContext(DbContextOptions options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Document> Documents { get; set; }
    }
}