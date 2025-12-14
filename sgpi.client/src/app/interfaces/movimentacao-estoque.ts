import { ItemCatalogo } from './item-catalogo';
import { Usuario } from './usuario';

export interface MovimentacaoEstoque {
  id: number;
  itemCatalogoId: number;
  itemCatalogo?: ItemCatalogo;
  tipoMovimentacao: TipoMovimentacao;
  quantidade: number;
  data: Date;
  usuarioId: string;
  usuario?: Usuario;

  // Optional fields
  ordemDeCompraId?: number;
  notaFiscal?: string;
  valorUnitarioEfetivo?: number;
  solicitante?: string;
  observacaoAjuste?: string;
}

export enum TipoMovimentacao {
  ENTRADA = 0,
  SAIDA = 1,
  AJUSTE_INVENTARIO = 2,
}

export interface RegistrarSaidaRequest {
  itemCatalogoId: number;
  quantidade: number;
  solicitante: string;
  userId?: string;
}

export interface RegistrarAjusteRequest {
  itemCatalogoId: number;
  novaQuantidade: number;
  observacao: string;
  userId?: string;
}
