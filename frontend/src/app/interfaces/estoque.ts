import { ItemCatalogo } from './item-catalogo';

export interface Estoque {
  id: number;
  itemCatalogoId: number;
  itemCatalogo?: ItemCatalogo;
  estoqueMinimo: number;
  estoqueMaximo: number;
  quantidadeEmEstoque: number;
}
