using SampleApi.Data;
using Microsoft.EntityFrameworkCore;
using SampleApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5270);
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    // Ensure database is created
    db.Database.EnsureCreated();

    // Seed data if empty
    if (!db.Users.Any())
    {
        db.Users.AddRange(
            new User { Username = "alice", Email = "alice@example.com" },
            new User { Username = "bob", Email = "bob@example.com" }
        );

        db.SaveChanges();
    }
}


app.MapGet("/", async (AppDbContext db) =>
{
    var users = await db.Users.ToListAsync();

    var html = "<h1>User List</h1><ul>";
    foreach (var user in users)
    {
        html += $"<li>{user.Username} - {user.Email}</li>";
    }
    html += "</ul>";

    return Results.Content(html, "text/html");
});

app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthorization();
app.MapControllers();
app.Run();
