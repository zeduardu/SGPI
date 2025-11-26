using Microsoft.EntityFrameworkCore;
using SGPI.Core.Entities;
using SGPI.Core.Interfaces;
using SGPI.Infrastructure.Data;

namespace SGPI.Infrastructure.Repositories
{
    public class OrdemCompraRepository : IOrdemCompraRepository
    {
        private readonly SgpiContext _context;

        public OrdemCompraRepository(SgpiContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OrdemDeCompra>> GetAllAsync()
        {
            return await _context.OrdensDeCompra
                .Include(o => o.Fornecedor)
                .Include(o => o.Itens)
                .ThenInclude(i => i.ItemCatalogo)
                .OrderByDescending(o => o.DataCriacao)
                .ToListAsync();
        }

        public async Task<OrdemDeCompra?> GetByIdAsync(int id)
        {
            return await _context.OrdensDeCompra
                .Include(o => o.Fornecedor)
                .Include(o => o.Itens)
                .ThenInclude(i => i.ItemCatalogo)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<OrdemDeCompra> AddAsync(OrdemDeCompra ordemCompra)
        {
            _context.OrdensDeCompra.Add(ordemCompra);
            await _context.SaveChangesAsync();
            return ordemCompra;
        }

        public async Task UpdateAsync(OrdemDeCompra ordemCompra)
        {
            _context.Entry(ordemCompra).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<OrdemDeCompra>> GetReportAsync(DateTime? start, DateTime? end, StatusOrdemDeCompra? status, string? requesterId)
        {
            var query = _context.OrdensDeCompra
                .Include(o => o.Fornecedor)
                .Include(o => o.UsuarioSolicitante)
                .Include(o => o.UsuarioAprovador)
                .Include(o => o.Itens)
                .ThenInclude(i => i.ItemCatalogo)
                .AsQueryable();

            if (start.HasValue) query = query.Where(o => o.DataCriacao >= start.Value);
            if (end.HasValue) query = query.Where(o => o.DataCriacao <= end.Value);
            if (status.HasValue) query = query.Where(o => o.Status == status.Value);
            if (!string.IsNullOrEmpty(requesterId)) query = query.Where(o => o.UsuarioSolicitanteId == requesterId);

            return await query.OrderByDescending(o => o.DataCriacao).ToListAsync();
        }
    }
}
