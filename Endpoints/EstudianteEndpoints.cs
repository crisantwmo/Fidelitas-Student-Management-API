using FidelitasAPI.Services;
using FidelitasAPI.Models.DTOs; 

public static class EstudianteEndpoints 
{
    public static void MapEstudianteEndpoints(this IEndpointRouteBuilder routes) 
    {
        var group = routes.MapGroup("/estudiantes");

        // GET: Devuelve ReadDto (oculta la estructura real de la DB)
        group.MapGet("/", async (EstudianteService service) => 
            Results.Ok(await service.ObtenerTodosAsync()));

        // POST: Recibe un CreateDto
        group.MapPost("/", async (EstudianteCreateDto nuevo, EstudianteService service) => {
            var res = await service.GuardarAsync(nuevo);
            return res is null ? Results.BadRequest("Datos o nota inválida (0-100)") : Results.Created($"/{res.Id}", res);
        });

        // PUT: Recibe el ID y el DTO con los nuevos datos
        group.MapPut("/{id:int}", async (int id, EstudianteCreateDto act, EstudianteService service) => 
            await service.ActualizarAsync(id, act) ? Results.NoContent() : Results.NotFound("Estudiante no encontrado"));

        // DELETE: (solo requiere el ID)
        group.MapDelete("/{id:int}", async (int id, EstudianteService service) => 
            await service.EliminarAsync(id) ? Results.Ok("Eliminado correctamente") : Results.NotFound());
    }
}