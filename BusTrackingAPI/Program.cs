using BusTrackingBL.Interfaces;
using BusTrackingBL.Services;
using BusTrackingDL.Models;
using BusTrackingDL.Models.Data;
using BusTrackingDL.Models.Entites;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDBContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IAdmin, AdminServices>();
builder.Services.AddScoped<IRouteService, RouteService>();
builder.Services.AddScoped<IStudent, StudentService>();
builder.Services.AddScoped<IDriver, DriverService>();
builder.Services.AddScoped<ITripService, TripService>();
builder.Services.AddScoped<IPasswordHasher<Admin>, PasswordHasher<Admin>>();
builder.Services.AddHostedService<BusTrackingAPI.BackgroundServices.TripBackgroundService>();
builder.Services.AddScoped<IStudentConfirmation,BusTrackingBL.Services.StudentConfirmation>();
builder.Services.AddControllers().AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter());
    });
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
