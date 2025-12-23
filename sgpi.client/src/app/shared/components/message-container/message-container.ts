import { Component, inject, OnInit } from '@angular/core';
import { AppMessage } from '@entities/app-message';
import { MessageService } from '@services/message.service';
import { Subject, takeUntil } from 'rxjs';
import { MatSnackBar, MatSnackBarHorizontalPosition, MatSnackBarVerticalPosition } from '@angular/material/snack-bar';
@Component({
  selector: 'app-message-container',
  imports: [],
  templateUrl: './message-container.html',
  styleUrl: './message-container.scss',
})
export class MessageContainer implements OnInit {
  messages: AppMessage[] = [];
  horizontalPosition: MatSnackBarHorizontalPosition = 'start';
  verticalPosition: MatSnackBarVerticalPosition = 'bottom';
  private destroy$ = new Subject<void>();
  private messageService: MessageService = inject(MessageService);
  private _snackBar = inject(MatSnackBar);


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

  clearAll(): void {
    this.messageService.clearAllMessages();
  }

  openSnackBar(message: string) {
    this._snackBar.open(message, 'Close', {
      horizontalPosition: this.horizontalPosition,
      verticalPosition: this.verticalPosition,
    });
  }
}
