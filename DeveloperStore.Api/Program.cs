using DeveloperStore.Application.Interfaces;
using DeveloperStore.Application.Services;
using DeveloperStore.Domain.Repositories;
using DeveloperStore.Infrastructure.Messaging;
using DeveloperStore.Infrastructure.Persistence;
using DeveloperStore.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Configure Log4Net
builder.Logging.ClearProviders();
builder.Logging.AddLog4Net("log4net.config");

// Configure services
builder.Services.AddDbContext<SalesDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
builder.Services.AddOpenApi();

builder.Services.AddScoped<IRabbitMQPublisher, MockRabbitMQPublisher>();
builder.Services.AddScoped<ISaleService, SaleService>();
builder.Services.AddScoped<ISaleRepository, SaleRepository>();

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "DeveloperStore API", Version = "v1" });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Configure middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

