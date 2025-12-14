namespace SGPI.Core.Entities
{
    public class ItemOrdemDeCompra
    {
        public int Id { get; set; }
        public int OrdemDeCompraId { get; set; }
        public OrdemDeCompra? OrdemDeCompra { get; set; }
        public int ItemCatalogoId { get; set; }
        public ItemCatalogo? ItemCatalogo { get; set; }
        public int QuantidadeSolicitada { get; set; }
        public int QuantidadeRecebida { get; set; }
        public decimal PrecoUnitario { get; set; }
    }
}
