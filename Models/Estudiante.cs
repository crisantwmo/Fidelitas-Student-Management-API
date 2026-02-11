namespace FidelitasAPI.Models;

public class Estudiante
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public double Nota { get; set; }
    public string Estado => Nota >= 70 ? "Aprobado" : "Reprobado";
}