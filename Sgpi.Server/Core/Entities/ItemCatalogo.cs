namespace SGPI.Core.Entities
{
    public class ItemCatalogo
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string DescricaoDetalhada { get; set; } = string.Empty;
        public UnidadeMedida UnidadeMedida { get; set; }
        public int CategoriaId { get; set; }
        public Categoria? Categoria { get; set; }
    }

    public enum UnidadeMedida
    {
        UN,
        KIT,
        METRO
    }
}
