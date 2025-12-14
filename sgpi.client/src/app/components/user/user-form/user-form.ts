import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { UserService } from '../../../services/user';
import { Usuario } from '../../../interfaces/usuario';

@Component({
  selector: 'app-user-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatCardModule,
    RouterLink,
  ],
  templateUrl: './user-form.html',
  styleUrls: ['./user-form.css'],
})
export class UserFormComponent implements OnInit {
  private fb = inject(FormBuilder);
  private userService = inject(UserService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  form: FormGroup = this.fb.group({
    nomeCompleto: ['', Validators.required],
    email: ['', [Validators.required, Validators.email]],
    cargo: ['', Validators.required],
    role: [''], // Optional role assignment
    password: [''], // Required only for new users
  });

  isEditMode = false;
  userId?: string;

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id && id !== 'new') {
      this.isEditMode = true;
      this.userId = id;
      this.loadUser(this.userId);
      this.form.get('password')?.clearValidators();
    } else {
      this.form.get('password')?.setValidators([Validators.required, Validators.minLength(6)]);
    }
    this.form.get('password')?.updateValueAndValidity();
  }

  loadUser(id: string): void {
    this.userService.getById(id).subscribe((user) => {
      this.form.patchValue(user);
    });
  }

  save(): void {
    if (this.form.invalid) return;

    const user: Usuario = {
      id: this.userId || '',
      ...this.form.value,
    };

    if (this.isEditMode) {
      this.userService.update(this.userId!, user).subscribe(() => {
        if (this.form.value.role) {
          this.userService.assignRole(this.userId!, this.form.value.role).subscribe(() => {
            this.router.navigate(['/users']);
          });
        } else {
          this.router.navigate(['/users']);
        }
      });
    } else {
      this.userService.create(user).subscribe((createdUser) => {
        if (this.form.value.role) {
          this.userService.assignRole(createdUser.id, this.form.value.role).subscribe(() => {
            this.router.navigate(['/users']);
          });
        } else {
          this.router.navigate(['/users']);
        }
      });
    }
  }
}
