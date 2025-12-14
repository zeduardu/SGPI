using Microsoft.EntityFrameworkCore;
using SGPI.Core.Entities;
using SGPI.Core.Interfaces;
using SGPI.Infrastructure.Data;

namespace SGPI.Infrastructure.Repositories
{
    public class MovimentacaoRepository : IMovimentacaoRepository
    {
        private readonly SgpiContext _context;

        public MovimentacaoRepository(SgpiContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MovimentacaoEstoque>> GetAllAsync()
        {
            return await _context.MovimentacoesEstoque
                .Include(m => m.ItemCatalogo)
                .Include(m => m.Usuario)
                .OrderByDescending(m => m.Data)
                .ToListAsync();
        }

        public async Task<IEnumerable<MovimentacaoEstoque>> GetByItemCatalogoIdAsync(int itemCatalogoId)
        {
            return await _context.MovimentacoesEstoque
                .Include(m => m.ItemCatalogo)
                .Include(m => m.Usuario)
                .Where(m => m.ItemCatalogoId == itemCatalogoId)
                .OrderByDescending(m => m.Data)
                .ToListAsync();
        }

        public async Task<MovimentacaoEstoque> AddAsync(MovimentacaoEstoque movimentacao)
        {
            _context.MovimentacoesEstoque.Add(movimentacao);
            await _context.SaveChangesAsync();
            return movimentacao;
        }

        public async Task<IEnumerable<MovimentacaoEstoque>> GetReportAsync(DateTime? start, DateTime? end, TipoMovimentacao? type, string? userId, int? itemId)
        {
            var query = _context.MovimentacoesEstoque
                .Include(m => m.ItemCatalogo)
                .Include(m => m.Usuario)
                .AsQueryable();

            if (start.HasValue) query = query.Where(m => m.Data >= start.Value);
            if (end.HasValue) query = query.Where(m => m.Data <= end.Value);
            if (type.HasValue) query = query.Where(m => m.TipoMovimentacao == type.Value);
            if (!string.IsNullOrEmpty(userId)) query = query.Where(m => m.UsuarioId == userId);
            if (itemId.HasValue) query = query.Where(m => m.ItemCatalogoId == itemId.Value);

            return await query.OrderByDescending(m => m.Data).ToListAsync();
        }
    }
}
