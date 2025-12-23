import { Fornecedor } from './fornecedor';
import { ItemCatalogo } from './item-catalogo';

export interface OrdemDeCompra {
  id?: number;
  fornecedorId: number;
  fornecedor?: Fornecedor;
  dataCriacao?: Date;
  usuarioSolicitanteId?: string;
  usuarioSolicitante?: any; // Using any to avoid circular dependency or import Usuario
  usuarioAprovadorId?: string;
  usuarioAprovador?: any;
  dataAprovacao?: Date;
  status: StatusOrdemDeCompra;
  observacao?: string;
  itens: ItemOrdemDeCompra[];
}

export interface ItemOrdemDeCompra {
  id?: number;
  ordemDeCompraId?: number;
  itemCatalogoId: number;
  itemCatalogo?: ItemCatalogo;
  quantidadeSolicitada: number;
  quantidadeRecebida: number;
  precoUnitario: number;
}

export enum StatusOrdemDeCompra {
  ABERTA = 0,
  APROVADA = 1,
  CONCLUIDA_PARCIAL = 2,
  CONCLUIDA_TOTAL = 3,
  CANCELADA = 4,
  REJEITADA = 5,
}

export interface ItemRecebimentoDto {
  itemCatalogoId: number;
  quantidade: number;
  valorUnitario: number;
}
