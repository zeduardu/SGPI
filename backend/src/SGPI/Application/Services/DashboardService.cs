using System.Collections.Generic;
using System.Threading.Tasks;
using SGPI.Core.Entities;
using SGPI.Core.Interfaces;

namespace SGPI.Application.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _dashboardRepository;

        public DashboardService(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }

        public async Task<IEnumerable<Estoque>> GetStockStatusAsync()
        {
            return await _dashboardRepository.GetStockAsync();
        }

        public async Task<IEnumerable<MovimentacaoEstoque>> GetRecentMovementsAsync(int count = 10)
        {
            return await _dashboardRepository.GetRecentMovementsAsync(count);
        }
    }
}
