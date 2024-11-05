using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MinimalApiLoggingApp;
using MinimalApiLoggingApp.Middlewares;
using MinimalApiLoggingApp.Models;
using MinimalApiLoggingApp.Services;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);

// Configure SQL Server connection
builder.Services.AddDbContext<ApiLoggingDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));



// Add services to the DI container
builder.Services.AddScoped<ILoggingService, LoggingService>();

// Register the BlacklistStore as a singleton
builder.Services.AddSingleton<BlacklistStore>();

// Background services 
builder.Services.AddScoped<ConfigurationService>();
builder.Services.AddHostedService<BlacklistUpdateService>();

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();

// Use Swagger
app.UseSwagger();
app.UseSwaggerUI();

// Use custom logging middleware
app.UseMiddleware<LoggingMiddleware>();

// Middleware to handle request from malicious IPs
app.UseMiddleware<BlacklistMiddleware>();



// Example endpoint for testing
app.MapGet("/api/test", () => "Hello, Minimal API!").WithName("TestEndpoint");

app.MapGet("/", () => "Welcome to the Minimal API!").WithName("RootEndpoint");

app.MapGet("/api/hello", (string name, int age) =>
{
    return Results.Ok($"Hello {name}, you are {age} years old.");
}).WithName("QueryParameterEndpoint");

app.MapPost("/api/person", (Person person) =>
{
    return Results.Ok($"Received person: {person.Name}, Age: {person.Age}");
}).WithName("PostPersonEndpoint");


app.Run();
