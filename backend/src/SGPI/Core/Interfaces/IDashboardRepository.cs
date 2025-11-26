using System.Collections.Generic;
using System.Threading.Tasks;
using SGPI.Core.Entities;

namespace SGPI.Core.Interfaces
{
    public interface IDashboardRepository
    {
        Task<IEnumerable<Estoque>> GetStockAsync();
        Task<IEnumerable<MovimentacaoEstoque>> GetRecentMovementsAsync(int count);
    }
}
