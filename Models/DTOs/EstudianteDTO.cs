using System.ComponentModel.DataAnnotations;

namespace FidelitasAPI.Models.DTOs;

public class EstudianteCreateDto
{
    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 100 caracteres")]
    public string Nombre { get; set; } = string.Empty;

    [Range(0, 100, ErrorMessage = "La nota debe estar entre 0 y 100")]
    public double Nota { get; set; }
}

public class EstudianteReadDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public double Nota { get; set; }
    public string Estado { get; set; } = string.Empty;
}