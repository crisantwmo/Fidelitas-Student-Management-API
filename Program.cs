using Microsoft.EntityFrameworkCore;
using FidelitasAPI.Services;
using FidelitasAPI.Data;
using FidelitasAPI.Middleware;

var builder = WebApplication.CreateBuilder(args);

// --- 1. CONFIGURACIÓN DE INFRAESTRUCTURA ---
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<EstudianteDbContext>(opt => {
    // Verificamos si la cadena apunta a PostgreSQL (Nube)
    if (connectionString != null && connectionString.Contains("Host=")) 
    {
        opt.UseNpgsql(connectionString);
    } 
    else 
    {
        // Si no detecta Host, usa SQLite (Local)
        // Usamos la cadena del JSON si existe, o el nombre por defecto
        var sqliteConn = (connectionString != null && connectionString.Contains("Data Source")) 
                         ? connectionString 
                         : "Data Source=estudiantes.db";
        opt.UseSqlite(sqliteConn);
    }
});

builder.Services.AddScoped<EstudianteService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// --- 2. MIDDLEWARES ---
app.UseMiddleware<ExceptionMiddleware>(); 

// Borramos o comentamos el 'if (app.Environment.IsDevelopment())'
// para que Swagger funcione siempre en Render
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Fidelitas API V1");
    c.RoutePrefix = string.Empty; // <-- Esto hará que la API abra Swagger directamente en la raíz
});

// Inicialización automática: Crea las tablas en Neon si no existen
using (var scope = app.Services.CreateScope()) {
    var dbContext = scope.ServiceProvider.GetRequiredService<EstudianteDbContext>();
    dbContext.Database.EnsureCreated();
}

// Registro de rutas
app.MapEstudianteEndpoints(); 

app.Run();