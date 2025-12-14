using Microsoft.EntityFrameworkCore;
using SGPI.Core.Entities;
using SGPI.Core.Interfaces;
using SGPI.Infrastructure.Data;

namespace SGPI.Infrastructure.Repositories
{
    public class EstoqueRepository : IEstoqueRepository
    {
        private readonly SgpiContext _context;

        public EstoqueRepository(SgpiContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Estoque>> GetAllAsync()
        {
            return await _context.Estoques
                .Include(e => e.ItemCatalogo)
                .ToListAsync();
        }

        public async Task<Estoque?> GetByIdAsync(int id)
        {
            return await _context.Estoques
                .Include(e => e.ItemCatalogo)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Estoque?> GetByItemCatalogoIdAsync(int itemCatalogoId)
        {
            return await _context.Estoques
                .Include(e => e.ItemCatalogo)
                .FirstOrDefaultAsync(e => e.ItemCatalogoId == itemCatalogoId);
        }

        public async Task<Estoque> AddAsync(Estoque estoque)
        {
            _context.Estoques.Add(estoque);
            await _context.SaveChangesAsync();
            return estoque;
        }

        public async Task UpdateAsync(Estoque estoque)
        {
            _context.Entry(estoque).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Estoque>> GetItemsBelowMinimumAsync()
        {
            return await _context.Estoques
                .Include(e => e.ItemCatalogo)
                .Where(e => e.QuantidadeEmEstoque < e.EstoqueMinimo)
                .ToListAsync();
        }

        public async Task DeleteAsync(Estoque estoque)
        {
            _context.Estoques.Remove(estoque);
            await _context.SaveChangesAsync();
        }
    }
}
