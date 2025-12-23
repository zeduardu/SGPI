export interface Usuario {
  id: string;
  email: string;
  nomeCompleto: string;
  cargo: string;
  password?: string; // Only for creation
  roles?: string[];
}
