using Microsoft.EntityFrameworkCore;
using FidelitasAPI.Models; 
using FidelitasAPI.Data;   
using FidelitasAPI.Models.DTOs; 

namespace FidelitasAPI.Services;

public class EstudianteService
{
    private readonly EstudianteDbContext _context;

    public EstudianteService(EstudianteDbContext context)
    {
        _context = context;
    }

    // Cambiamos para que devuelva una lista de ReadDto
    public async Task<List<EstudianteReadDto>> ObtenerTodosAsync() 
    {
        var estudiantes = await _context.Estudiantes.ToListAsync();
        return estudiantes.Select(e => new EstudianteReadDto 
        {
            Id = e.Id,
            Nombre = e.Nombre,
            Nota = e.Nota,
            Estado = e.Estado
        }).ToList();
    }

    // Ahora recibe un CreateDto y devuelve un ReadDto
    public async Task<EstudianteReadDto?> GuardarAsync(EstudianteCreateDto nuevoDto)
    {
        if (nuevoDto.Nota < 0 || nuevoDto.Nota > 100) return null;

        // Mapeo: De DTO a Entidad (Base de datos)
        var estudiante = new Estudiante 
        {
            Nombre = nuevoDto.Nombre,
            Nota = nuevoDto.Nota
        };

        _context.Estudiantes.Add(estudiante);
        await _context.SaveChangesAsync();

        // Mapeo: De Entidad a DTO de respuesta
        return new EstudianteReadDto
        {
            Id = estudiante.Id,
            Nombre = estudiante.Nombre,
            Nota = estudiante.Nota,
            Estado = estudiante.Estado
        };
    }

    // DTO para los datos de entrada
    public async Task<bool> ActualizarAsync(int id, EstudianteCreateDto actualizadoDto)
    {
        var estudiante = await _context.Estudiantes.FindAsync(id);
        if (estudiante is null) return false;

        if (actualizadoDto.Nota < 0 || actualizadoDto.Nota > 100) return false;

        estudiante.Nombre = actualizadoDto.Nombre;
        estudiante.Nota = actualizadoDto.Nota;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var estudiante = await _context.Estudiantes.FindAsync(id);
        if (estudiante is null) return false;

        _context.Estudiantes.Remove(estudiante);
        await _context.SaveChangesAsync();
        return true;
    }
}