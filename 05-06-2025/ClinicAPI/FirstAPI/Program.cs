using System.Text;
using FirstApi.Contexts;
using FirstApi.Interfaces;
using FirstApi.Models;
using FirstApi.Services;
using FirstApi.Repositories;
using Microsoft.EntityFrameworkCore;
using FirstApi.Misc;
using Microsoft.AspNetCore.Authentication.Google; 
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authorization;
using FirstApi.Policies.Handlers;
using FirstApi.Authorization;
using Microsoft.AspNetCore.Authentication;
using IAuthenticationService = FirstApi.Interfaces.IAuthenticationService;
using AuthenticationService = FirstApi.Services.AuthenticationService;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Features;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "Clinic API", Version = "v1" });
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});
builder.Services.AddControllers()
                .AddJsonOptions(opts =>
                {
                    opts.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
                    opts.JsonSerializerOptions.WriteIndented = true;
                });

builder.Logging.AddLog4Net();

builder.Services.AddDbContext<ClinicContext>(opts =>
{
    opts.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});


#region  Repositories
builder.Services.AddTransient<IRepository<int, Doctor>, DoctorRepository>();
builder.Services.AddTransient<IRepository<int, Speciality>, SpecialityRepository>();
builder.Services.AddTransient<IRepository<int, DoctorSpeciality>, DoctorSpecialityRepository>();
builder.Services.AddTransient<IRepository<int, Patient>, PatientRepository>();
builder.Services.AddTransient<IRepository<string, Appointment>, AppointmentRepository>();
builder.Services.AddTransient<IRepository<string, User>, UserRepository>();

#endregion


#region Services
builder.Services.AddTransient<IDoctorService, DoctorService>();
builder.Services.AddTransient<IPatientService, PatientService>();
// builder.Services.AddTransient<IDoctorService, DoctorServiceWithTransaction>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddTransient<IOtherContextFunctionalities, OtherFuncinalitiesImplementation>();
builder.Services.AddTransient<IEncryptionService, EncryptionService>();
builder.Services.AddTransient<ITokenService, TokenService>();
builder.Services.AddTransient<IAuthenticationService, AuthenticationService>();
#endregion


#region AuthenticationFilter
// builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//                 .AddJwtBearer(options =>
//                 {
//                     options.TokenValidationParameters = new TokenValidationParameters
//                     {
//                         ValidateAudience = false,
//                         ValidateIssuer = false,
//                         ValidateLifetime = true,
//                         ValidateIssuerSigningKey = true,
//                         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Keys:JwtTokenKey"]))
//                     };
//                 })
//                 .AddCookie()
//                 .AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
//                 {
//                     options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
//                     options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
//                 });

builder.Services.AddAuthentication(options =>
                    {
                        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                        options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
                    })
                    .AddCookie()
                    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateAudience = false,
                            ValidateIssuer = false,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Keys:JwtTokenKey"]))
                        };
                    })
                    .AddGoogle(options =>
                    {
                        options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
                        options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
                    });
#endregion


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("DoctorExperienceCheck", policy =>
        policy.Requirements.Add(new HasEnoughExperienceRequirement(3)));
});

builder.Services.AddScoped<IAuthorizationHandler, HasEnoughExperienceHandler>();

#region  Misc
builder.Services.AddAutoMapper(typeof(User));
builder.Services.AddAutoMapper(typeof(PatientMapping));
builder.Services.AddAutoMapper(typeof(AppointmentMapper));
builder.Services.AddScoped<CustomExceptionFilter>();
#endregion




#region CORS
builder.Services.AddCors(options=>{
    options.AddDefaultPolicy(policy=>{
        policy.WithOrigins("http://127.0.0.1:5500")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});
#endregion

builder.Services.AddSignalR();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseCors();
app.MapHub<NotificationHub>("/notificationhub");

app.MapControllers();

app.Run();