namespace SGPI.Core.Entities
{
    public class Estoque
    {
        public int Id { get; set; }
        public int ItemCatalogoId { get; set; }
        public ItemCatalogo? ItemCatalogo { get; set; }
        public int EstoqueMinimo { get; set; }
        public int EstoqueMaximo { get; set; }
        public int QuantidadeEmEstoque { get; set; }
    }
}
