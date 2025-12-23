import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatSelectModule } from '@angular/material/select';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { EstoqueService } from '@services/estoque';
import { ItemCatalogoService } from '@services/item-catalogo';
import { Estoque } from '@entities/estoque';
import { ItemCatalogo } from '@entities/item-catalogo';

@Component({
  selector: 'app-estoque-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatInputModule,
    MatButtonModule,
    MatCardModule,
    MatSelectModule,
    RouterLink,
  ],
  templateUrl: './estoque-form.html',
  styleUrls: ['./estoque-form.scss'],
})
export class EstoqueFormComponent implements OnInit {
  private fb = inject(FormBuilder);
  private estoqueService = inject(EstoqueService);
  private itemService = inject(ItemCatalogoService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  form: FormGroup = this.fb.group({
    itemCatalogoId: [null, Validators.required],
    estoqueMinimo: [0, [Validators.required, Validators.min(0)]],
    estoqueMaximo: [0, [Validators.required, Validators.min(0)]],
    quantidadeEmEstoque: [0, [Validators.required, Validators.min(0)]],
  });

  isEditMode = false;
  estoqueId?: number;
  itens: ItemCatalogo[] = [];

  ngOnInit(): void {
    this.loadItens();
    const id = this.route.snapshot.paramMap.get('id');
    if (id && id !== 'new') {
      this.isEditMode = true;
      this.estoqueId = +id;
      this.loadEstoque(this.estoqueId);
    }
  }

  loadItens(): void {
    this.itemService.getAll().subscribe((data) => {
      this.itens = data;
    });
  }

  loadEstoque(id: number): void {
    this.estoqueService.getById(id).subscribe((estoque) => {
      this.form.patchValue(estoque);
    });
  }

  save(): void {
    if (this.form.invalid) return;

    const estoque: Estoque = {
      id: this.estoqueId || 0,
      ...this.form.value,
    };

    if (this.isEditMode) {
      this.estoqueService.update(this.estoqueId!, estoque).subscribe(() => {
        this.router.navigate(['/estoque']);
      });
    } else {
      this.estoqueService.create(estoque).subscribe(() => {
        this.router.navigate(['/estoque']);
      });
    }
  }
}
