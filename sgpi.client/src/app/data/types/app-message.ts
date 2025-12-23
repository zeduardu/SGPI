export interface AppMessage {
  id: string;
  severity: 'success' | 'info' | 'warn' | 'error';
  summary: string;
  detail?: string;
  life?: number; // Duration in milliseconds
  closable?: boolean; // Whether the message can be closed by the user
}