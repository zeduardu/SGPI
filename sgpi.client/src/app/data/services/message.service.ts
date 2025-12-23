import { Injectable } from '@angular/core';
import { AppMessage } from '@entities/app-message';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class MessageService {
  private messagesSubject = new BehaviorSubject<AppMessage[]>([]);
  messages$: Observable<AppMessage[]> = this.messagesSubject.asObservable();

  constructor() {}

  private generateId(): string {
    return Math.random().toString(36).substring(2, 15);
  }

  addMessage(message: Omit<AppMessage, 'id'>): void {
    const newMessage: AppMessage = {
      ...message,
      id: this.generateId(),
      life: message.life || 5000,
      closable: message.closable !== false
    };

    const currentMessages = this.messagesSubject.value;
    this.messagesSubject.next([...currentMessages, newMessage]);

    if (newMessage.life && newMessage.life > 0) {
      setTimeout(() => {
        this.removeMessage(newMessage.id);
      }, newMessage.life);
    }
  }

  removeMessage(id: string): void {
    const currentMessages = this.messagesSubject.value;
    const updatedMessages = currentMessages.filter(message => message.id !== id);
    this.messagesSubject.next(updatedMessages);
  }

  clearAllMessages(): void {
    this.messagesSubject.next([]);
  }

  // Métodos convenientes para diferentes tipos de mensagem
  showSuccess(summary: string, detail?: string, life?: number): void {
    this.addMessage({ severity: 'success', summary, detail, life });
  }

  showInfo(summary: string, detail?: string, life?: number): void {
    this.addMessage({ severity: 'info', summary, detail, life });
  }

  showWarn(summary: string, detail?: string, life?: number): void {
    this.addMessage({ severity: 'warn', summary, detail, life });
  }

  showError(summary: string, detail?: string, life?: number): void {
    this.addMessage({ severity: 'error', summary, detail, life });
  }
}
