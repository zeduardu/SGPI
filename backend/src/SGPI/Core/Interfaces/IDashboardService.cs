using System.Collections.Generic;
using System.Threading.Tasks;
using SGPI.Core.Entities;

namespace SGPI.Core.Interfaces
{
    public interface IDashboardService
    {
        Task<IEnumerable<Estoque>> GetStockStatusAsync();
        Task<IEnumerable<MovimentacaoEstoque>> GetRecentMovementsAsync(int count = 10);
    }
}
