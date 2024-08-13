using AuthenticationService.Models;
using AuthenticationService.Repositories;
using AuthenticationService.Repositories.CustomerRepositories;
using AuthenticationService.Services;
using MessageBrokerService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure database context
builder.Services.AddDbContext<AuthenticateContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register scoped services
builder.Services.AddScoped<JwtTokenHandler>();
builder.Services.AddScoped<ICustomerRepo, CustomerRepo>();

// Register singleton services
builder.Services.AddSingleton<BaseMessageBroker>();
builder.Services.AddSingleton<CustomerAuthenticationConsumer>();
builder.Services.AddSingleton<IHostedService>(provider => provider.GetRequiredService<CustomerAuthenticationConsumer>());

// Configure CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins("http://localhost:1000")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials());
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowSpecificOrigin");
app.UseAuthorization();
app.MapControllers();

app.Run();
