using Microsoft.EntityFrameworkCore;
using FidelitasAPI.Models;

namespace FidelitasAPI.Data;

public class EstudianteDbContext : DbContext
{
    public EstudianteDbContext(DbContextOptions<EstudianteDbContext> options) : base(options) { }

    public DbSet<Estudiante> Estudiantes => Set<Estudiante>();
}