import { Categoria } from './categoria';

export interface ItemCatalogo {
  id: number;
  nome: string;
  descricaoDetalhada: string;
  unidadeMedida: UnidadeMedida;
  categoriaId: number;
  categoria?: Categoria;
}

export enum UnidadeMedida {
  UN = 0,
  KIT = 1,
  METRO = 2,
}
