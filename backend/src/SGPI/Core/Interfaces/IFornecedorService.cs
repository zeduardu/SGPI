using SGPI.Core.Entities;

namespace SGPI.Core.Interfaces
{
    public interface IFornecedorService
    {
        Task<IEnumerable<Fornecedor>> GetAllFornecedoresAsync();
        Task<Fornecedor?> GetFornecedorByIdAsync(int id);
        Task<Fornecedor> CreateFornecedorAsync(Fornecedor fornecedor);
        Task UpdateFornecedorAsync(int id, Fornecedor fornecedor);
        Task DeleteFornecedorAsync(int id);
    }
}
