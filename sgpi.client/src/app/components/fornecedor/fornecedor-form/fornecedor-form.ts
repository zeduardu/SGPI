import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { FornecedorService } from '@services/fornecedor';
import { Fornecedor } from '../../../data/types/fornecedor';

@Component({
  selector: 'app-fornecedor-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatInputModule,
    MatButtonModule,
    MatCardModule,
    RouterLink,
  ],
  templateUrl: './fornecedor-form.html',
  styleUrls: ['./fornecedor-form.scss'],
})
export class FornecedorFormComponent implements OnInit {
  private fb = inject(FormBuilder);
  private fornecedorService = inject(FornecedorService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  form: FormGroup = this.fb.group({
    nome: ['', [Validators.required, Validators.minLength(3)]],
    emailContato: ['', [Validators.email]],
    telefoneContato: [''],
    cnpj: [''],
  });

  isEditMode = false;
  fornecedorId?: number;

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id && id !== 'new') {
      this.isEditMode = true;
      this.fornecedorId = +id;
      this.loadFornecedor(this.fornecedorId);
    }
  }

  loadFornecedor(id: number): void {
    this.fornecedorService.getById(id).subscribe((fornecedor) => {
      this.form.patchValue(fornecedor);
    });
  }

  save(): void {
    if (this.form.invalid) return;

    const fornecedor: Fornecedor = {
      id: this.fornecedorId || 0,
      ...this.form.value,
    };

    if (this.isEditMode) {
      this.fornecedorService.update(this.fornecedorId!, fornecedor).subscribe(() => {
        this.router.navigate(['/fornecedores']);
      });
    } else {
      this.fornecedorService.create(fornecedor).subscribe(() => {
        this.router.navigate(['/fornecedores']);
      });
    }
  }
}
