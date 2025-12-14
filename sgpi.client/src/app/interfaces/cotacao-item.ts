import { ItemCatalogo } from './item-catalogo';
import { Fornecedor } from './fornecedor';

export interface CotacaoItem {
  id: number;
  itemCatalogoId: number;
  itemCatalogo?: ItemCatalogo;
  fornecedorId: number;
  fornecedor?: Fornecedor;
  precoUnitario: number;
  dataCotacao: Date;
  linkProduto?: string;
}
