import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { CategoriaService } from '@services/categoria';
import { Categoria } from '../../../data/types/categoria';

@Component({
  selector: 'app-categoria-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatInputModule,
    MatButtonModule,
    MatCardModule,
    RouterLink,
  ],
  templateUrl: './categoria-form.html',
  styleUrls: ['./categoria-form.scss'],
})
export class CategoriaFormComponent implements OnInit {
  private fb = inject(FormBuilder);
  private categoriaService = inject(CategoriaService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  form: FormGroup = this.fb.group({
    nome: ['', [Validators.required, Validators.minLength(3)]],
  });

  isEditMode = false;
  categoriaId?: number;

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id && id !== 'new') {
      this.isEditMode = true;
      this.categoriaId = +id;
      this.loadCategoria(this.categoriaId);
    }
  }

  loadCategoria(id: number): void {
    this.categoriaService.getById(id).subscribe((categoria) => {
      this.form.patchValue(categoria);
    });
  }

  save(): void {
    if (this.form.invalid) return;

    const categoria: Categoria = {
      id: this.categoriaId || 0,
      ...this.form.value,
    };

    if (this.isEditMode) {
      this.categoriaService.update(this.categoriaId!, categoria).subscribe(() => {
        this.router.navigate(['/categorias']);
      });
    } else {
      this.categoriaService.create(categoria).subscribe(() => {
        this.router.navigate(['/categorias']);
      });
    }
  }
}
