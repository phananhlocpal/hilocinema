using Microsoft.EntityFrameworkCore;
using SaleService.Models;
using SaleService.Repositories.FoodRepository;
using SaleService.Repositories.InvoiceFoodRepository;
using SaleService.Repositories.InvoiceRepository;
using SaleService.Service.RabbitMQServices;
using SaleService.Services.VNPayService;
using Microsoft.Extensions.Caching.StackExchangeRedis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Register distributed Redis cache
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379";
    options.InstanceName = "SaleServiceSession";
});

// Configure session to use Redis
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.Name = "SaleServiceSession";
    // Optionally set other session options here
});

// Register DbContext
builder.Services.AddDbContext<SaleContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register HttpClient
builder.Services.AddHttpClient("ScheduleService", client =>
{
    client.BaseAddress = new Uri("https://localhost:5003/api/Schedule/");
});

// Register AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Register repositories
builder.Services.AddScoped<IFoodRepo, FoodRepo>();
builder.Services.AddScoped<IInvoiceFoodRepo, InvoiceFoodRepo>();
builder.Services.AddScoped<IInvoiceRepo, InvoiceRepo>();

// Register services
builder.Services.AddSingleton<IVnPayService, VnPayService>();
builder.Services.AddScoped<SalePublisherService>();

// Register HttpContextAccessor
builder.Services.AddHttpContextAccessor();

builder.Services.AddSwaggerGen();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:2000")
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseSession();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();
