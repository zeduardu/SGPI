import { Component, inject, OnInit, OnDestroy } from '@angular/core';
import { AppMessage } from '@entities/app-message';
import { MessageService } from '@services/message.service';
import { Subject, takeUntil } from 'rxjs';

@Component({
  selector: 'app-message-container',
  imports: [],
  templateUrl: './message-container.html',
  styleUrl: './message-container.scss',
})
export class MessageContainer implements OnInit, OnDestroy {
  messages: AppMessage[] = [];
  private destroy$ = new Subject<void>();
  private messageService: MessageService = inject(MessageService);

  constructor() {}

  ngOnInit(): void {
    this.messageService.messages$
      .pipe(takeUntil(this.destroy$))
      .subscribe(messages => {
        this.messages = messages;
      });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  onCloseMessage(id: string): void {
    this.messageService.removeMessage(id);
  }
}
