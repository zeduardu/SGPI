using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using SGPI.Core.Entities;
using SGPI.Core.Interfaces;

public static class FornecedorEndpoints
{
    public static void MapFornecedorEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/fornecedores").WithTags("Fornecedores");

        group.MapGet("/", async (IFornecedorService service) =>
        {
            return Results.Ok(await service.GetAllFornecedoresAsync());
        })
        .WithName("GetAllFornecedores")
        .Produces<IEnumerable<Fornecedor>>(StatusCodes.Status200OK);

        group.MapGet("/{id}", async (int id, IFornecedorService service) =>
        {
            var fornecedor = await service.GetFornecedorByIdAsync(id);
            return fornecedor is not null ? Results.Ok(fornecedor) : Results.NotFound();
        })
        .WithName("GetFornecedorById")
        .Produces<Fornecedor>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        group.MapPost("/", async (Fornecedor fornecedor, IFornecedorService service) =>
        {
            var createdFornecedor = await service.CreateFornecedorAsync(fornecedor);
            return Results.Created($"/api/fornecedores/{createdFornecedor.Id}", createdFornecedor);
        })
        .WithName("CreateFornecedor")
        .Produces<Fornecedor>(StatusCodes.Status201Created);

        group.MapPut("/{id}", async (int id, Fornecedor fornecedor, IFornecedorService service) =>
        {
            if (id != fornecedor.Id)
            {
                return Results.BadRequest();
            }

            try
            {
                await service.UpdateFornecedorAsync(id, fornecedor);
                return Results.NoContent();
            }
            catch (KeyNotFoundException)
            {
                return Results.NotFound();
            }
        })
        .WithName("UpdateFornecedor")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status400BadRequest);

        group.MapDelete("/{id}", async (int id, IFornecedorService service) =>
        {
            await service.DeleteFornecedorAsync(id);
            return Results.NoContent();
        })
        .WithName("DeleteFornecedor")
        .Produces(StatusCodes.Status204NoContent);
    }
}
