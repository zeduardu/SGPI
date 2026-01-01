import { Component, inject } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { AuthService } from '../../auth/auth.service';
import { MessageService } from '../../data/services/message.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-forgot-password-dialog',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule
  ],
  templateUrl: './forgot-password-dialog.html',
  styles: [`
    .w-full { width: 100%; }
    mat-form-field { width: 100%; }
  `]
})
export class ForgotPasswordDialog {
  private authService = inject(AuthService);
  private messageService = inject(MessageService);
  private dialogRef = inject(MatDialogRef<ForgotPasswordDialog>);

  forgotPasswordForm = new FormGroup({
    email: new FormControl('', [Validators.required, Validators.email])
  });

  isLoading = false;

  submit() {
    if (this.forgotPasswordForm.invalid) return;

    this.isLoading = true;
    const email = this.forgotPasswordForm.value.email!;

    this.authService.forgotPassword(email).subscribe({
      next: () => {
        this.messageService.showSuccess('Sucesso', 'Um link de recuperação foi enviado para seu email.');
        this.isLoading = false;
        this.dialogRef.close();
      },
      error: (err) => {
        this.messageService.showError('Erro', 'Não foi possível enviar o email de recuperação. Tente novamente.');
        this.isLoading = false;
      }
    });
  }
}
