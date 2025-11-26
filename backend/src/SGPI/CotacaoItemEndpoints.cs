using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using SGPI.Core.Entities;
using SGPI.Core.Interfaces;

public static class CotacaoItemEndpoints
{
    public static void MapCotacaoItemEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/cotacoes").WithTags("Cotacoes");

        group.MapGet("/", async (ICotacaoItemService service) =>
        {
            return Results.Ok(await service.GetAllCotacoesAsync());
        })
        .WithName("GetAllCotacoes")
        .Produces<IEnumerable<CotacaoItem>>(StatusCodes.Status200OK);

        group.MapGet("/{id}", async (int id, ICotacaoItemService service) =>
        {
            var cotacao = await service.GetCotacaoByIdAsync(id);
            return cotacao is not null ? Results.Ok(cotacao) : Results.NotFound();
        })
        .WithName("GetCotacaoById")
        .Produces<CotacaoItem>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        group.MapGet("/item/{itemCatalogoId}", async (int itemCatalogoId, ICotacaoItemService service) =>
        {
            var cotacoes = await service.GetCotacoesByItemCatalogoIdAsync(itemCatalogoId);
            return Results.Ok(cotacoes);
        })
        .WithName("GetCotacoesByItemCatalogoId")
        .Produces<IEnumerable<CotacaoItem>>(StatusCodes.Status200OK);

        group.MapPost("/", async (CotacaoItem cotacao, ICotacaoItemService service) =>
        {
            var createdCotacao = await service.CreateCotacaoAsync(cotacao);
            return Results.Created($"/api/cotacoes/{createdCotacao.Id}", createdCotacao);
        })
        .WithName("CreateCotacao")
        .Produces<CotacaoItem>(StatusCodes.Status201Created);

        group.MapPut("/{id}", async (int id, CotacaoItem cotacao, ICotacaoItemService service) =>
        {
            if (id != cotacao.Id)
            {
                return Results.BadRequest();
            }

            try
            {
                await service.UpdateCotacaoAsync(id, cotacao);
                return Results.NoContent();
            }
            catch (KeyNotFoundException)
            {
                return Results.NotFound();
            }
        })
        .WithName("UpdateCotacao")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status400BadRequest);

        group.MapDelete("/{id}", async (int id, ICotacaoItemService service) =>
        {
            await service.DeleteCotacaoAsync(id);
            return Results.NoContent();
        })
        .WithName("DeleteCotacao")
        .Produces(StatusCodes.Status204NoContent);
    }
}
