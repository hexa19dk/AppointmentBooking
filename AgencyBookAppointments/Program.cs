using AgencyBookAppointments.Data;
using AgencyBookAppointments.Repositories;
using AgencyBookAppointments.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Services
builder.Services.AddScoped<IAgencyService, AgencyService>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();

// Repositories
builder.Services.AddScoped<IAgencyRepositories, AgencyRepositories>();
builder.Services.AddScoped<IAppointmentRepositories, AppointmentRepositories>();
builder.Services.AddScoped<IHolidayRepositories, HolidayRepositories>();
builder.Services.AddScoped<IConfigurationRepositories, ConfigurationRepositories>();


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
