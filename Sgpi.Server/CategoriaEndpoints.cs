using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using SGPI.Core.Entities;
using SGPI.Core.Interfaces;

public static class CategoriaEndpoints
{
    public static void MapCategoriaEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/categorias").WithTags("Categorias");

        group.MapGet("/", async (ICategoriaService service) =>
        {
            return Results.Ok(await service.GetAllCategoriasAsync());
        })
        .WithName("GetAllCategorias")
        .Produces<IEnumerable<Categoria>>(StatusCodes.Status200OK);

        group.MapGet("/{id}", async (int id, ICategoriaService service) =>
        {
            var categoria = await service.GetCategoriaByIdAsync(id);
            return categoria is not null ? Results.Ok(categoria) : Results.NotFound();
        })
        .WithName("GetCategoriaById")
        .Produces<Categoria>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        group.MapPost("/", async (Categoria categoria, ICategoriaService service) =>
        {
            var createdCategoria = await service.CreateCategoriaAsync(categoria);
            return Results.Created($"/api/categorias/{createdCategoria.Id}", createdCategoria);
        })
        .WithName("CreateCategoria")
        .Produces<Categoria>(StatusCodes.Status201Created);

        group.MapPut("/{id}", async (int id, Categoria categoria, ICategoriaService service) =>
        {
            if (id != categoria.Id)
            {
                return Results.BadRequest();
            }

            try
            {
                await service.UpdateCategoriaAsync(id, categoria);
                return Results.NoContent();
            }
            catch (KeyNotFoundException)
            {
                return Results.NotFound();
            }
        })
        .WithName("UpdateCategoria")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status400BadRequest);

        group.MapDelete("/{id}", async (int id, ICategoriaService service) =>
        {
            await service.DeleteCategoriaAsync(id);
            return Results.NoContent();
        })
        .WithName("DeleteCategoria")
        .Produces(StatusCodes.Status204NoContent);
    }
}
