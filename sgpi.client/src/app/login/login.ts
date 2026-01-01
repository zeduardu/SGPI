import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { AuthService } from '../auth/auth.service';
import { Router } from '@angular/router';
import {MatCheckbox} from '@angular/material/checkbox';
import { MessageContainer } from "../shared/components/message-container/message-container";
import { MatDialog } from '@angular/material/dialog';
import { ForgotPasswordDialog } from './forgot-password-dialog/forgot-password-dialog';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, MatCardModule, MatInputModule, MatButtonModule, MatCheckbox, MessageContainer],
  templateUrl: './login.html',
  styleUrl: './login.scss',
})
export class Login {
  loginForm = new FormGroup({
    email: new FormControl('', [Validators.required, Validators.email]),
    password: new FormControl('', Validators.required),
  });

  constructor(private authService: AuthService, private router: Router, private dialog: MatDialog) {}

  onSubmit() {
    if (this.loginForm.valid) {
      this.authService.login(this.loginForm.value as any).subscribe({
        next: (response) => {
          localStorage.setItem('token', response.token);
          this.router.navigate(['/dashboard']);
        },
        error: (err) => {
          // console.error('Login failed', err);
        },
      });
    }
  }

  openForgotPasswordDialog() {
    this.dialog.open(ForgotPasswordDialog, {
      width: '400px'
    });
  }
}
