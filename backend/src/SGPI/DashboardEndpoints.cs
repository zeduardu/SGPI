using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using SGPI.Core.Interfaces;
using SGPI.Core.Entities;

public static class DashboardEndpoints
{
    public static void MapDashboardEndpoints(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/api/dashboard/stock-status", async (IDashboardService dashboardService) =>
        {
            var stockStatus = await dashboardService.GetStockStatusAsync();
            return Results.Ok(stockStatus);
        })
        .WithName("GetStockStatus")
        .Produces<IResult>(StatusCodes.Status200OK);

        routes.MapGet("/api/dashboard/recent-movements", async (IDashboardService dashboardService) =>
        {
            var movements = await dashboardService.GetRecentMovementsAsync();
            return Results.Ok(movements);
        })
        .WithName("GetRecentMovements")
        .Produces<IEnumerable<MovimentacaoEstoque>>(StatusCodes.Status200OK);
    }
}
