using Microsoft.EntityFrameworkCore;
using SGPI.Core.Entities;
using SGPI.Core.Interfaces;
using SGPI.Infrastructure.Data;

namespace SGPI.Application.Services
{
    public class MovimentacaoService : IMovimentacaoService
    {
        private readonly IMovimentacaoRepository _movimentacaoRepository;
        private readonly IEstoqueRepository _estoqueRepository;
        private readonly SgpiContext _context;

        public MovimentacaoService(
            IMovimentacaoRepository movimentacaoRepository,
            IEstoqueRepository estoqueRepository,
            SgpiContext context)
        {
            _movimentacaoRepository = movimentacaoRepository;
            _estoqueRepository = estoqueRepository;
            _context = context;
        }

        public async Task<IEnumerable<MovimentacaoEstoque>> GetAllAsync()
        {
            return await _movimentacaoRepository.GetAllAsync();
        }

        public async Task<IEnumerable<MovimentacaoEstoque>> GetByItemCatalogoIdAsync(int itemCatalogoId)
        {
            return await _movimentacaoRepository.GetByItemCatalogoIdAsync(itemCatalogoId);
        }

        public async Task RegistrarSaidaAsync(int itemCatalogoId, int quantidade, string usuarioId, string solicitante)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var estoque = await _estoqueRepository.GetByItemCatalogoIdAsync(itemCatalogoId);
                if (estoque == null)
                {
                    throw new InvalidOperationException($"Estoque não encontrado para o item {itemCatalogoId}");
                }

                if (estoque.QuantidadeEmEstoque < quantidade)
                {
                    throw new InvalidOperationException($"Saldo insuficiente. Disponível: {estoque.QuantidadeEmEstoque}, Solicitado: {quantidade}");
                }

                // Decrement stock
                estoque.QuantidadeEmEstoque -= quantidade;
                await _estoqueRepository.UpdateAsync(estoque);

                // Create movement record
                var movimentacao = new MovimentacaoEstoque
                {
                    ItemCatalogoId = itemCatalogoId,
                    TipoMovimentacao = TipoMovimentacao.SAIDA,
                    Quantidade = quantidade,
                    Data = DateTime.UtcNow,
                    UsuarioId = usuarioId,
                    Solicitante = solicitante
                };

                await _movimentacaoRepository.AddAsync(movimentacao);

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task RegistrarAjusteAsync(int itemCatalogoId, int novaQuantidade, string usuarioId, string observacao)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var estoque = await _estoqueRepository.GetByItemCatalogoIdAsync(itemCatalogoId);
                if (estoque == null)
                {
                    // If stock record doesn't exist, create it? 
                    // For now, assume it must exist. If not, we might need to create it.
                    // Let's assume it exists for simplicity as per current flow.
                     throw new InvalidOperationException($"Estoque não encontrado para o item {itemCatalogoId}");
                }

                var diferenca = novaQuantidade - estoque.QuantidadeEmEstoque;

                // Update stock
                estoque.QuantidadeEmEstoque = novaQuantidade;
                await _estoqueRepository.UpdateAsync(estoque);

                // Create movement record
                var movimentacao = new MovimentacaoEstoque
                {
                    ItemCatalogoId = itemCatalogoId,
                    TipoMovimentacao = TipoMovimentacao.AJUSTE_INVENTARIO,
                    Quantidade = diferenca, // Can be positive or negative
                    Data = DateTime.UtcNow,
                    UsuarioId = usuarioId,
                    ObservacaoAjuste = observacao
                };

                await _movimentacaoRepository.AddAsync(movimentacao);

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
