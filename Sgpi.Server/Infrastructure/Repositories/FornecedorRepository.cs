using Microsoft.EntityFrameworkCore;
using SGPI.Core.Entities;
using SGPI.Core.Interfaces;
using SGPI.Infrastructure.Data;

namespace SGPI.Infrastructure.Repositories
{
    public class FornecedorRepository : IFornecedorRepository
    {
        private readonly SgpiContext _context;

        public FornecedorRepository(SgpiContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Fornecedor>> GetAllAsync()
        {
            return await _context.Fornecedores.ToListAsync();
        }

        public async Task<Fornecedor?> GetByIdAsync(int id)
        {
            return await _context.Fornecedores.FindAsync(id);
        }

        public async Task<Fornecedor> AddAsync(Fornecedor fornecedor)
        {
            _context.Fornecedores.Add(fornecedor);
            await _context.SaveChangesAsync();
            return fornecedor;
        }

        public async Task UpdateAsync(Fornecedor fornecedor)
        {
            _context.Entry(fornecedor).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Fornecedor fornecedor)
        {
            _context.Fornecedores.Remove(fornecedor);
            await _context.SaveChangesAsync();
        }
    }
}
