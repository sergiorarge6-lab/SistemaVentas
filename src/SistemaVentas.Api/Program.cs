using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using SistemaVentas.Api.BackgroundServices;
using SistemaVentas.Api.Middleware;
using SistemaVentas.Application.Interfaces;
using SistemaVentas.Application.Interfaces.Security;
using SistemaVentas.Application.Services;
using SistemaVentas.Infrastructure.Cache;
using SistemaVentas.Infrastructure.Data;
using SistemaVentas.Infrastructure.Repositories;
using SistemaVentas.Infrastructure.Security;
using System.Text;

//lo pongo antes del builder para que capture los mensajes del arranque de la aplicacion
Log.Logger = new LoggerConfiguration()

    .MinimumLevel.Information()
    .WriteTo.File(
        "logs/log-.txt",    
        rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

Log.Information("Aplicación SistemaVentas iniciada.");

builder.Services.AddScoped<IProductoService, ProductoService>();

// ADO.NET
//builder.Services.AddScoped<IProductoRepository, ProductoRepository>();

// Entity Framework Core
builder.Services.AddScoped<IProductoRepository, ProductoEfRepository>();


builder.Services.AddScoped<IPedidoService, PedidoService>();

//ADO.NET
//builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();

// Entity Framework Core
builder.Services.AddScoped<IPedidoRepository, PedidoEfRepository>();


builder.Services.AddScoped<IJwtService, JwtService>();

builder.Services.AddScoped<ICacheService, MemoryCacheService>();


// agrego services al container.
builder.Services.AddDbContext<SistemaVentasDbContext>((serviceProvider, options) =>
{
    var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

    options.UseSqlServer(
        builder.Configuration.GetConnectionString("SistemaVentas"));

    options.EnableSensitiveDataLogging();

    options.UseLoggerFactory(loggerFactory);
});

// JWT
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
 
               
                ValidIssuer =
                    builder.Configuration["Jwt:Issuer"],

                ValidAudience =
                    builder.Configuration["Jwt:Audience"],

                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            builder.Configuration["Jwt:Key"]!))
            };
    });

//para el BackGrond Service
builder.Services.AddHostedService<PedidoBackgroundService>();

builder.Services.AddAuthorization();


builder.Services.AddControllers();

builder.Services.AddHealthChecks();

//para IMemoryCache
builder.Services.AddMemoryCache();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "SistemaVentas API",
        Version = "v1"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingrese únicamente el JWT."
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

// https://localhost:44305/health
app.MapHealthChecks("/health");

app.Run();
