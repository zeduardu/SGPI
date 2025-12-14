using Microsoft.EntityFrameworkCore;
using SGPI.Core.Entities;
using SGPI.Core.Interfaces;
using SGPI.Infrastructure.Data;

namespace SGPI.Infrastructure.Repositories
{
    public class ItemCatalogoRepository : IItemCatalogoRepository
    {
        private readonly SgpiContext _context;

        public ItemCatalogoRepository(SgpiContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ItemCatalogo>> GetAllAsync()
        {
            return await _context.ItensCatalogo
                .Include(i => i.Categoria)
                .ToListAsync();
        }

        public async Task<ItemCatalogo?> GetByIdAsync(int id)
        {
            return await _context.ItensCatalogo
                .Include(i => i.Categoria)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<ItemCatalogo> AddAsync(ItemCatalogo item)
        {
            _context.ItensCatalogo.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task UpdateAsync(ItemCatalogo item)
        {
            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(ItemCatalogo item)
        {
            _context.ItensCatalogo.Remove(item);
            await _context.SaveChangesAsync();
        }
    }
}
