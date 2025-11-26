using SGPI.Core.Entities;
using SGPI.Core.Interfaces;

namespace SGPI.Application.Services
{
    public class FornecedorService : IFornecedorService
    {
        private readonly IFornecedorRepository _fornecedorRepository;

        public FornecedorService(IFornecedorRepository fornecedorRepository)
        {
            _fornecedorRepository = fornecedorRepository;
        }

        public async Task<IEnumerable<Fornecedor>> GetAllFornecedoresAsync()
        {
            return await _fornecedorRepository.GetAllAsync();
        }

        public async Task<Fornecedor?> GetFornecedorByIdAsync(int id)
        {
            return await _fornecedorRepository.GetByIdAsync(id);
        }

        public async Task<Fornecedor> CreateFornecedorAsync(Fornecedor fornecedor)
        {
            return await _fornecedorRepository.AddAsync(fornecedor);
        }

        public async Task UpdateFornecedorAsync(int id, Fornecedor fornecedor)
        {
            var existingFornecedor = await _fornecedorRepository.GetByIdAsync(id);
            if (existingFornecedor == null)
            {
                throw new KeyNotFoundException($"Fornecedor with ID {id} not found.");
            }

            existingFornecedor.Nome = fornecedor.Nome;
            existingFornecedor.EmailContato = fornecedor.EmailContato;
            existingFornecedor.TelefoneContato = fornecedor.TelefoneContato;
            existingFornecedor.CNPJ = fornecedor.CNPJ;

            await _fornecedorRepository.UpdateAsync(existingFornecedor);
        }

        public async Task DeleteFornecedorAsync(int id)
        {
            var fornecedor = await _fornecedorRepository.GetByIdAsync(id);
            if (fornecedor != null)
            {
                await _fornecedorRepository.DeleteAsync(fornecedor);
            }
        }
    }
}
