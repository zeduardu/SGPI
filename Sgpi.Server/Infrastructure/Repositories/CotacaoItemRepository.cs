using Microsoft.EntityFrameworkCore;
using SGPI.Core.Entities;
using SGPI.Core.Interfaces;
using SGPI.Infrastructure.Data;

namespace SGPI.Infrastructure.Repositories
{
    public class CotacaoItemRepository : ICotacaoItemRepository
    {
        private readonly SgpiContext _context;

        public CotacaoItemRepository(SgpiContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CotacaoItem>> GetAllAsync()
        {
            return await _context.CotacoesItens
                .Include(c => c.ItemCatalogo)
                .Include(c => c.Fornecedor)
                .ToListAsync();
        }

        public async Task<CotacaoItem?> GetByIdAsync(int id)
        {
            return await _context.CotacoesItens
                .Include(c => c.ItemCatalogo)
                .Include(c => c.Fornecedor)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<CotacaoItem>> GetByItemCatalogoIdAsync(int itemCatalogoId)
        {
            return await _context.CotacoesItens
                .Include(c => c.ItemCatalogo)
                .Include(c => c.Fornecedor)
                .Where(c => c.ItemCatalogoId == itemCatalogoId)
                .ToListAsync();
        }

        public async Task<CotacaoItem> AddAsync(CotacaoItem cotacao)
        {
            _context.CotacoesItens.Add(cotacao);
            await _context.SaveChangesAsync();
            return cotacao;
        }

        public async Task UpdateAsync(CotacaoItem cotacao)
        {
            _context.Entry(cotacao).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(CotacaoItem cotacao)
        {
            _context.CotacoesItens.Remove(cotacao);
            await _context.SaveChangesAsync();
        }
    }
}
