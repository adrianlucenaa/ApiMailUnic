
using APICruzber.Datos;
using APICruzber.Interfaces;
using APICruzber.Controllers;
using APICruzber.Modelo;
using APICruzber.Connection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Logging;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Configuración de logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "APICruzber", Version = "v1" });
    /*
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    */
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            new string[] { }
        }
    });
});

builder.Configuration.AddJsonFile("appsettings.json");
var Key = builder.Configuration.GetSection("jwt").GetSection("Key").ToString();
var keyBytes = Encoding.UTF8.GetBytes(Key);

builder.Services.AddAuthentication(config =>
{
    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(config =>
{
    config.RequireHttpsMetadata = false;
    config.SaveToken = true;
    config.TokenValidationParameters = new TokenValidationParameters
    {
        
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("estosoloesunaclavedepruebaadmincruzber08")),
        ValidateIssuer = false,
        ValidateAudience = false,
       
    };
});

builder.Services.AddScoped<ConnectionBD>();
//builder.Services.AddScoped<DatosCliente>();
builder.Services.AddScoped<DatosMail>();
//builder.Services.AddScoped<ICliente, DatosCliente>();

// Inyección del servicio de logging en DatosMail
builder.Services.AddScoped<IEmail, DatosMail>(provider =>
{
    var connectionString = builder.Configuration.GetConnectionString("ConnectionStrings:ConexionBD");
    var logger = provider.GetRequiredService<ILogger<DatosMail>>();
    return new DatosMail(new ConnectionBD(), logger);
});

builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
