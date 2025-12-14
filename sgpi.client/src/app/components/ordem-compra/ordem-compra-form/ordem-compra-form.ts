import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, FormArray, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { Router, RouterModule } from '@angular/router';
import { OrdemCompraService } from '../../../services/ordem-compra';
import { FornecedorService } from '../../../services/fornecedor';
import { ItemCatalogoService } from '../../../services/item-catalogo';
import { Fornecedor } from '../../../interfaces/fornecedor';
import { ItemCatalogo } from '../../../interfaces/item-catalogo';

@Component({
  selector: 'app-ordem-compra-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatIconModule,
    MatCardModule,
    RouterModule,
  ],
  templateUrl: './ordem-compra-form.html',
  styleUrl: './ordem-compra-form.css',
})
export class OrdemCompraFormComponent implements OnInit {
  form: FormGroup;
  fornecedores: Fornecedor[] = [];
  itensCatalogo: ItemCatalogo[] = [];

  constructor(
    private fb: FormBuilder,
    private ordemCompraService: OrdemCompraService,
    private fornecedorService: FornecedorService,
    private itemCatalogoService: ItemCatalogoService,
    private router: Router
  ) {
    this.form = this.fb.group({
      fornecedorId: ['', Validators.required],
      observacao: [''],
      itens: this.fb.array([]),
    });
  }

  ngOnInit(): void {
    this.loadFornecedores();
    this.loadItensCatalogo();

    const state = history.state;
    if (state && state.items && Array.isArray(state.items)) {
      state.items.forEach((item: any) => {
        this.addItem(item);
      });
    } else {
      this.addItem(); // Start with one empty item row if no state
    }
  }

  get itens(): FormArray {
    return this.form.get('itens') as FormArray;
  }

  loadFornecedores(): void {
    this.fornecedorService.getAll().subscribe((data) => (this.fornecedores = data));
  }

  loadItensCatalogo(): void {
    this.itemCatalogoService.getAll().subscribe((data) => (this.itensCatalogo = data));
  }

  addItem(itemData?: any): void {
    const itemGroup = this.fb.group({
      itemCatalogoId: [itemData?.itemCatalogoId || '', Validators.required],
      quantidadeSolicitada: [itemData?.quantidade || 1, [Validators.required, Validators.min(1)]],
      precoUnitario: [itemData?.precoUnitario || 0, [Validators.required, Validators.min(0)]],
    });
    this.itens.push(itemGroup);
  }

  removeItem(index: number): void {
    this.itens.removeAt(index);
  }

  onSubmit(): void {
    if (this.form.valid) {
      this.ordemCompraService.create(this.form.value).subscribe({
        next: () => {
          this.router.navigate(['/ordem-compra']);
        },
        error: (err) => {
          console.error('Error creating OC', err);
        },
      });
    }
  }
}
