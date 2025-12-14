using SGPI.Core.Entities;

namespace SGPI.Core.Interfaces
{
    public interface IFornecedorRepository
    {
        Task<IEnumerable<Fornecedor>> GetAllAsync();
        Task<Fornecedor?> GetByIdAsync(int id);
        Task<Fornecedor> AddAsync(Fornecedor fornecedor);
        Task UpdateAsync(Fornecedor fornecedor);
        Task DeleteAsync(Fornecedor fornecedor);
    }
}
