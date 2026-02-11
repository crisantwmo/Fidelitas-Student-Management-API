using Microsoft.EntityFrameworkCore;
using FidelitasAPI.Services;
using FidelitasAPI.Data;
using FidelitasAPI.Middleware;

var builder = WebApplication.CreateBuilder(args);

// 1. Registro de Infraestructura
builder.Services.AddDbContext<EstudianteDbContext>(opt => 
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<EstudianteService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// --- 2. CONFIGURACIÓN DEL PIPELINE (Middlewares) ---

// EL ESCUDO PROTECTOR
app.UseMiddleware<ExceptionMiddleware>(); 

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Inicialización de la base de datos
using (var scope = app.Services.CreateScope()) {
    scope.ServiceProvider.GetRequiredService<EstudianteDbContext>().Database.EnsureCreated();
}

// Registro de Endpoints
app.MapEstudianteEndpoints(); 

app.Run();