using Application.Services;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Mediator;
using Domain.Strategies;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using FluentValidation.AspNetCore;
using Application.Dtos;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Application.Services.Mappings;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services.AddControllers();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    });

// Configuración de Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    // Configuración del esquema de seguridad
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Bearer token authentication",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey
    });

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

// Configuración de servicios (repositorios, AutoMapper, etc.)
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<EmployeeService>();
builder.Services.AddScoped<IBonusStrategy, RegularEmployeeBonusStrategy>();
builder.Services.AddScoped<IRequestHandler<GetEmployeeRequest, Employee>, GetEmployeeHandler>();
builder.Services.AddScoped<IPositionHistoryRepository, PositionHistoryRepository>();
builder.Services.AddScoped<PositionHistoryService>();
builder.Services.AddScoped<IRequestHandler<GetPositionHistoryRequest, PositionHistory>, GetPositionHistoryHandler>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<AuthService>();  
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<ProjectService>();  
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<DepartmentService>(); 
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddAutoMapper(amc => amc.AddProfile<AutoMapping>());

// Configuración de la base de datos
builder.Services.AddDbContext<EmployeeDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<PositionHistoryDto>();

// Configuración de autenticación y JWT
builder.Services.AddAuthentication("Bearer")
.AddJwtBearer("Bearer", options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        //ValidIssuer = "https://pruebasAPI.com", //Se configura el emisor del token que será válido para que el API acepte el token
        //ValidAudience = "https://pruebasAPP.com", //Se configura el receptor del token válido, el que solicitó el token 
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("uBtAbOs/RgDuGK7+NnzRgHvX5Gt6lgg/OG5UYnfwJmw=")),
        RoleClaimType = ClaimTypes.Role
    };

    //código comentado debido a que aún no logro hacer que funcione la personalización de las respuestas cuando falla la autenticación
    //options.Events = new JwtBearerEvents
    //{
    //    OnAuthenticationFailed = context =>
    //    {
    //        context.Response.StatusCode = 401;
    //        context.Response.ContentType = "application/json";
    //        var result = JsonSerializer.Serialize(new { message = "Token inválido" });
    //        return context.Response.WriteAsync(result);
    //    },
    //    OnChallenge = context =>
    //    {
    //        context.HandleResponse();
    //        context.Response.StatusCode = 401;
    //        var result = JsonSerializer.Serialize(new { message = "El token de autenticación es requerido" });
    //        return context.Response.WriteAsync(result);
    //    },
    //    OnForbidden = context =>
    //    {
    //        context.Response.StatusCode = 403;
    //        context.Response.ContentType = "application/json";
    //        var result = JsonSerializer.Serialize(new { message = "No tienes permisos suficientes para ver los empleados." });
    //        return context.Response.WriteAsync(result);
    //    }
    //};
});

//código comentado debido a que aún no logro hacer que funcione el control de la autorización según los roles desde las políticas de autorización y el rol que viene en el token
//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("Admin", policy =>
//    policy.RequireClaim(ClaimTypes.Role, "1"));
//});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.Use(async (context, next) =>
{
    await next();

    if (context.Response.StatusCode == 401)
    {
        context.Response.ContentType = "application/json";
        var result = JsonSerializer.Serialize(new { message = "El token de autenticación es requerido" });
        await context.Response.WriteAsync(result);
    }

    if (context.Response.StatusCode == 403)
    {
        context.Response.ContentType = "application/json";
        var result = JsonSerializer.Serialize(new { message = "No tienes permisos suficientes para ver los empleados." });
        await context.Response.WriteAsync(result);
    }
});

app.MapControllers();

app.Run();
