// using ShopApi.Data;
// using Microsoft.EntityFrameworkCore;
// using AutoMapper;
// using ShopApi.Mappings;
// using ShopApi.Interfaces;
// using ShopApi.Services.Interfaces;
// using ShopApi.Repositories;


// var builder = WebApplication.CreateBuilder(args);

// // Add services to the container
// builder.Services.AddControllers();

// builder.Services.AddDbContext<AppDbContext>(options =>
//     options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();
// // Add AutoMapper
// builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

// builder.Services.AddScoped<IProductService, ProductService>();
// builder.Services.AddAutoMapper(typeof(Program));

// builder.Services.AddScoped<IProductRepository, ProductRepository>();
// var app = builder.Build();

// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

// app.UseHttpsRedirection();

// app.UseAuthorization();

// app.MapControllers();

// app.Run();

using ShopApi.Data;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using ShopApi.Mappings;
using ShopApi.Interfaces;
using ShopApi.Repositories;
using ShopApi.Services;
using ShopApi.Services.Interfaces;
using ShopAPI.Repositories;
using ShopApi.Repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

//  Repositories
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IColorRepository, ColorRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();

// Services 
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IColorService, ColorService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICheckoutService, CheckoutService>();
builder.Services.AddScoped<INewsService, NewsService>();

var app = builder.Build();

// Swagger UI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middlewares
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
