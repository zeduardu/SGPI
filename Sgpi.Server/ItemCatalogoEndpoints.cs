using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using SGPI.Core.Entities;
using SGPI.Core.Interfaces;

public static class ItemCatalogoEndpoints
{
    public static void MapItemCatalogoEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/itens-catalogo").WithTags("ItensCatalogo");

        group.MapGet("/", async (IItemCatalogoService service) =>
        {
            return Results.Ok(await service.GetAllItensAsync());
        })
        .WithName("GetAllItensCatalogo")
        .Produces<IEnumerable<ItemCatalogo>>(StatusCodes.Status200OK);

        group.MapGet("/{id}", async (int id, IItemCatalogoService service) =>
        {
            var item = await service.GetItemByIdAsync(id);
            return item is not null ? Results.Ok(item) : Results.NotFound();
        })
        .WithName("GetItemCatalogoById")
        .Produces<ItemCatalogo>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        group.MapPost("/", async (ItemCatalogo item, IItemCatalogoService service) =>
        {
            var createdItem = await service.CreateItemAsync(item);
            return Results.Created($"/api/itens-catalogo/{createdItem.Id}", createdItem);
        })
        .WithName("CreateItemCatalogo")
        .Produces<ItemCatalogo>(StatusCodes.Status201Created);

        group.MapPut("/{id}", async (int id, ItemCatalogo item, IItemCatalogoService service) =>
        {
            if (id != item.Id)
            {
                return Results.BadRequest();
            }

            try
            {
                await service.UpdateItemAsync(id, item);
                return Results.NoContent();
            }
            catch (KeyNotFoundException)
            {
                return Results.NotFound();
            }
        })
        .WithName("UpdateItemCatalogo")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status400BadRequest);

        group.MapDelete("/{id}", async (int id, IItemCatalogoService service) =>
        {
            await service.DeleteItemAsync(id);
            return Results.NoContent();
        })
        .WithName("DeleteItemCatalogo")
        .Produces(StatusCodes.Status204NoContent);
    }
}
