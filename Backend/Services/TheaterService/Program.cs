using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TheaterService.Models;
using TheaterService.Service;

var builder = WebApplication.CreateBuilder(args);

// Configuration
var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<TheaterContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

// Add scoped services

// Add auto mapper services
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Add hosted and factory services


// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:1001") 
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

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();
