using Microsoft.EntityFrameworkCore;
using BankingApi.Models;

public class BankingContext : DbContext
{
    public BankingContext(DbContextOptions<BankingContext> options) : base(options) { }
    public DbSet<Account> Accounts { get; set; }
}
