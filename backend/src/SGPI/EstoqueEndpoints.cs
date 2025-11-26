using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using SGPI.Core.Entities;
using SGPI.Core.Interfaces;

public static class EstoqueEndpoints
{
    public static void MapEstoqueEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/estoque").WithTags("Estoque");

        group.MapGet("/", async (IEstoqueService service) =>
        {
            return Results.Ok(await service.GetAllEstoqueAsync());
        })
        .WithName("GetAllEstoque")
        .Produces<IEnumerable<Estoque>>(StatusCodes.Status200OK);

        group.MapGet("/{id}", async (int id, IEstoqueService service) =>
        {
            var estoque = await service.GetEstoqueByIdAsync(id);
            return estoque is not null ? Results.Ok(estoque) : Results.NotFound();
        })
        .WithName("GetEstoqueById")
        .Produces<Estoque>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        group.MapGet("/item/{itemCatalogoId}", async (int itemCatalogoId, IEstoqueService service) =>
        {
            var estoque = await service.GetEstoqueByItemCatalogoIdAsync(itemCatalogoId);
            return estoque is not null ? Results.Ok(estoque) : Results.NotFound();
        })
        .WithName("GetEstoqueByItemCatalogoId")
        .Produces<Estoque>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        group.MapPost("/", async (Estoque estoque, IEstoqueService service) =>
        {
            var createdEstoque = await service.CreateEstoqueAsync(estoque);
            return Results.Created($"/api/estoque/{createdEstoque.Id}", createdEstoque);
        })
        .WithName("CreateEstoque")
        .Produces<Estoque>(StatusCodes.Status201Created);

        group.MapPut("/{id}", async (int id, Estoque estoque, IEstoqueService service) =>
        {
            if (id != estoque.Id)
            {
                return Results.BadRequest();
            }

            try
            {
                await service.UpdateEstoqueAsync(id, estoque);
                return Results.NoContent();
            }
            catch (KeyNotFoundException)
            {
                return Results.NotFound();
            }
        })
        .WithName("UpdateEstoque")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status400BadRequest);

        group.MapDelete("/{id}", async (int id, IEstoqueService service) =>
        {
            await service.DeleteEstoqueAsync(id);
            return Results.NoContent();
        })
        .WithName("DeleteEstoque")
        .Produces(StatusCodes.Status204NoContent);

        group.MapGet("/shopping-list", async (IEstoqueService service) =>
        {
            return Results.Ok(await service.GetItemsBelowMinimumAsync());
        })
        .WithName("GetShoppingList")
        .Produces<IEnumerable<Estoque>>(StatusCodes.Status200OK);
    }
}
