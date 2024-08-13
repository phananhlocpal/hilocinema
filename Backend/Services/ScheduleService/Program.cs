using Microsoft.EntityFrameworkCore;
using ScheduleService.Repositories.ScheduleRepository;
using ScheduleService.Models;
using ScheduleService.Service.HttpServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register HttpClient for MovieService
builder.Services.AddHttpClient("MovieService", client =>
{
    client.BaseAddress = new Uri("https://localhost:5002/api/Movies/");
});

builder.Services.AddHttpClient("SeatService", client =>
{
    client.BaseAddress = new Uri("http://localhost:5010/api/Seats/"); 
});

builder.Services.AddHttpClient("TheaterService", client =>
{
    client.BaseAddress = new Uri("http://localhost:5010/api/Theaters/"); 
});

builder.Services.AddHttpClient("RoomService", client =>
{
    client.BaseAddress = new Uri("http://localhost:5010/api/Rooms/"); 
});
builder.Services.AddHttpClient("InvoiceService", client =>
{
    client.BaseAddress = new Uri("http://localhost:5004/api/Invoice/"); 
});

// Configure database context
builder.Services.AddDbContext<ScheduleContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Register repositories and services
builder.Services.AddScoped<IScheduleRepo, ScheduleRepo>();
builder.Services.AddScoped<MovieHttpService>();
builder.Services.AddScoped<TheaterHttpService>();
builder.Services.AddScoped<InvoiceHttpService>();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:5001") // Update this URL to match your React app's URL
               .AllowAnyHeader()
               .AllowAnyMethod();
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

app.UseAuthorization();

app.MapControllers();

app.Run();
