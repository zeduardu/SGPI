using SGPI.Core.Entities;
using SGPI.Core.Interfaces;

namespace SGPI.Application.Services
{
    public class CotacaoItemService : ICotacaoItemService
    {
        private readonly ICotacaoItemRepository _cotacaoRepository;

        public CotacaoItemService(ICotacaoItemRepository cotacaoRepository)
        {
            _cotacaoRepository = cotacaoRepository;
        }

        public async Task<IEnumerable<CotacaoItem>> GetAllCotacoesAsync()
        {
            return await _cotacaoRepository.GetAllAsync();
        }

        public async Task<CotacaoItem?> GetCotacaoByIdAsync(int id)
        {
            return await _cotacaoRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<CotacaoItem>> GetCotacoesByItemCatalogoIdAsync(int itemCatalogoId)
        {
            return await _cotacaoRepository.GetByItemCatalogoIdAsync(itemCatalogoId);
        }

        public async Task<CotacaoItem> CreateCotacaoAsync(CotacaoItem cotacao)
        {
            // Ensure relationships are not set to avoid EF trying to insert them if they are just ID references
            cotacao.ItemCatalogo = null;
            cotacao.Fornecedor = null;
            cotacao.DataCotacao = cotacao.DataCotacao.ToUniversalTime(); // Ensure UTC
            return await _cotacaoRepository.AddAsync(cotacao);
        }

        public async Task UpdateCotacaoAsync(int id, CotacaoItem cotacao)
        {
            var existingCotacao = await _cotacaoRepository.GetByIdAsync(id);
            if (existingCotacao == null)
            {
                throw new KeyNotFoundException($"Cotacao with ID {id} not found.");
            }

            existingCotacao.ItemCatalogoId = cotacao.ItemCatalogoId;
            existingCotacao.FornecedorId = cotacao.FornecedorId;
            existingCotacao.PrecoUnitario = cotacao.PrecoUnitario;
            existingCotacao.DataCotacao = cotacao.DataCotacao.ToUniversalTime();
            existingCotacao.LinkProduto = cotacao.LinkProduto;

            await _cotacaoRepository.UpdateAsync(existingCotacao);
        }

        public async Task DeleteCotacaoAsync(int id)
        {
            var cotacao = await _cotacaoRepository.GetByIdAsync(id);
            if (cotacao != null)
            {
                await _cotacaoRepository.DeleteAsync(cotacao);
            }
        }
    }
}
