using System.Text;
using EventManagementAPI.Data;
using EventManagementAPI.Interfaces;
using EventManagementAPI.Mappings;
using EventManagementAPI.Misc;
using EventManagementAPI.Services;
using EventManagementAPI.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using EventManagementAPI.Hubs;
using Serilog;
using Microsoft.OpenApi.Models;
using AspNetCoreRateLimit;
using Microsoft.Extensions.FileProviders;
using Serilog.Sinks.AzureBlobStorage;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

var builder = WebApplication.CreateBuilder(args);

// var keyVaultUrl = builder.Configuration["AzureVaultSettings:VaultUri"];
// builder.Configuration.AddAzureKeyVault(
//     new Uri(keyVaultUrl),
//     new DefaultAzureCredential());


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "EventManagementAPI", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsIn...\""
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});


builder.Services.AddOptions();
builder.Services.AddMemoryCache();
builder.Services.Configure<IpRateLimitOptions>(options =>
{
    options.GeneralRules = new List<RateLimitRule>
    {
        new RateLimitRule
        {
            Endpoint = "*",
            Period = "10s",
            Limit = 100
        }
    };
});

builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
builder.Services.AddInMemoryRateLimiting();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddAutoMapper(typeof(MappingProfiles));
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.AddScoped<IJwtService, JwtService>();

#region Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IRegistrationRepository, RegistrationRepository>();
builder.Services.AddScoped<IWalletRepository, WalletRepository>();
builder.Services.AddScoped<IWalletTransactionsRepository, WalletTransactionsRepository>();
#endregion

#region Services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IRegistrationService, RegistrationService>();
builder.Services.AddScoped<IWalletService, WalletService>();
// builder.Services.AddScoped<IBlobService, BlobService>();
builder.Services.AddScoped<IEmailService, EmailService>();
#endregion




var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = jwtSettings.Issuer,

        ValidateAudience = true,
        ValidAudience = jwtSettings.Audience,

        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
    };

    options.Events = new JwtBearerEvents
    {
        OnForbidden = async context =>
        {
            context.Response.StatusCode = 403;
            context.Response.ContentType = "application/json";

            var response = System.Text.Json.JsonSerializer.Serialize(new { Message = "You do not have permission to perform this action." });
            await context.Response.WriteAsync(response);
        }
    };
});


#region CORS
builder.Services.AddCors(options => {
    options.AddDefaultPolicy(policy => {
        policy.WithOrigins("http://127.0.0.1:5501")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .SetIsOriginAllowed(_ => true);
    });
});
#endregion



#region Logging
builder.Host.UseSerilog((ctx, lc) =>
    lc.WriteTo.Console()
      .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day));

// builder.Host.UseSerilog((ctx, lc) =>
// {
//     lc.WriteTo.Console()
//       .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
//       .WriteTo.AzureBlobStorage(
//           connectionString: builder.Configuration["AzureBlobSettingsFS:ConnectionString"],
//           storageContainerName: "event-logs"
//       );
// });

// builder.Host.UseSerilog((ctx, lc) =>
// {
//     lc.WriteTo.Console()
//       .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
//       .WriteTo.AzureBlobStorage(
//           connectionString: ctx.Configuration["eventAPI-BlobConnectionString"], 
//           storageContainerName: "event-logs"
//       );
// });

#endregion



builder.Services.AddSignalR();
builder.Services.AddAuthorization();
builder.Services.AddControllers();


var app = builder.Build();


if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles")),
    RequestPath = "/UploadedFiles"
});

// app.UseHttpsRedirection();

app.UseCors();
app.MapHub<EventHub>("/eventHub");
app.UseIpRateLimiting();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();