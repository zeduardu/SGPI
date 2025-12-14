using System;

namespace SGPI.Core.Entities
{
    public class CotacaoItem
    {
        public int Id { get; set; }
        public int ItemCatalogoId { get; set; }
        public ItemCatalogo? ItemCatalogo { get; set; }
        public int FornecedorId { get; set; }
        public Fornecedor? Fornecedor { get; set; }
        public decimal PrecoUnitario { get; set; }
        public DateTime DataCotacao { get; set; }
        public string? LinkProduto { get; set; }
    }
}
