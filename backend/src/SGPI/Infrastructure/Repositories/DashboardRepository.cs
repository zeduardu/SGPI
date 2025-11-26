using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SGPI.Core.Entities;
using SGPI.Core.Interfaces;
using SGPI.Infrastructure.Data;

namespace SGPI.Infrastructure.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly SgpiContext _context;

        public DashboardRepository(SgpiContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Estoque>> GetStockAsync()
        {
            return await _context.Estoques
                .Include(e => e.ItemCatalogo!)
                .ThenInclude(ic => ic.Categoria)
                .ToListAsync();
        }

        public async Task<IEnumerable<MovimentacaoEstoque>> GetRecentMovementsAsync(int count)
        {
            return await _context.MovimentacoesEstoque
                .Include(m => m.ItemCatalogo)
                .Include(m => m.Usuario)
                .OrderByDescending(m => m.Data)
                .Take(count)
                .ToListAsync();
        }
    }
}
