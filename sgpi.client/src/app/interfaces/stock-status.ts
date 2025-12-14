export interface StockStatus {
    id: number;
    itemCatalogoId: number;
    itemCatalogo?: {
        id: number;
        nome: string;
        descricaoDetalhada: string;
        unidadeMedida: number;
        categoriaId: number;
        categoria?: {
            id: number;
            nome: string;
        }
    };
    estoqueMinimo: number;
    estoqueMaximo: number;
    quantidadeEmEstoque: number;
    status: string; // This will be calculated in the frontend
}
