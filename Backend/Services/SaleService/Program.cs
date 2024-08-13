var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register HttpClient
builder.Services.AddHttpClient("ScheduleService", client =>
{
    client.BaseAddress = new Uri("https://localhost:5003/api/Schedule/"); // Ensure this URL is correct
});

builder.Services.AddHttpClient("EmployeeService", client =>
{
    client.BaseAddress = new Uri("https://localhost:5006/api/Employee/"); // Ensure this URL is correct
});

builder.Services.AddHttpClient("CustomerService", client =>
{
    client.BaseAddress = new Uri("https://localhost:5005/api/Custimer/"); // Ensure this URL is correct
});

// Register AutoMap
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
