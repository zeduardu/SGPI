import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { CotacaoItemService } from '../../../services/cotacao-item';
import { ItemCatalogoService } from '../../../services/item-catalogo';
import { FornecedorService } from '../../../services/fornecedor';
import { CotacaoItem } from '../../../interfaces/cotacao-item';
import { ItemCatalogo } from '../../../interfaces/item-catalogo';
import { Fornecedor } from '../../../interfaces/fornecedor';

@Component({
  selector: 'app-cotacao-item-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatInputModule,
    MatButtonModule,
    MatCardModule,
    MatSelectModule,
    MatDatepickerModule,
    MatNativeDateModule,
    RouterLink,
  ],
  templateUrl: './cotacao-item-form.html',
  styleUrls: ['./cotacao-item-form.css'],
})
export class CotacaoItemFormComponent implements OnInit {
  private fb = inject(FormBuilder);
  private cotacaoService = inject(CotacaoItemService);
  private itemService = inject(ItemCatalogoService);
  private fornecedorService = inject(FornecedorService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  form: FormGroup = this.fb.group({
    itemCatalogoId: [null, Validators.required],
    fornecedorId: [null, Validators.required],
    precoUnitario: [0, [Validators.required, Validators.min(0.01)]],
    dataCotacao: [new Date(), Validators.required],
    linkProduto: [''],
  });

  isEditMode = false;
  cotacaoId?: number;
  itens: ItemCatalogo[] = [];
  fornecedores: Fornecedor[] = [];

  ngOnInit(): void {
    this.loadItens();
    this.loadFornecedores();
    const id = this.route.snapshot.paramMap.get('id');
    if (id && id !== 'new') {
      this.isEditMode = true;
      this.cotacaoId = +id;
      this.loadCotacao(this.cotacaoId);
    }
  }

  loadItens(): void {
    this.itemService.getAll().subscribe((data) => {
      this.itens = data;
    });
  }

  loadFornecedores(): void {
    this.fornecedorService.getAll().subscribe((data) => {
      this.fornecedores = data;
    });
  }

  loadCotacao(id: number): void {
    this.cotacaoService.getById(id).subscribe((cotacao) => {
      this.form.patchValue(cotacao);
    });
  }

  save(): void {
    if (this.form.invalid) return;

    const cotacao: CotacaoItem = {
      id: this.cotacaoId || 0,
      ...this.form.value,
    };

    if (this.isEditMode) {
      this.cotacaoService.update(this.cotacaoId!, cotacao).subscribe(() => {
        this.router.navigate(['/cotacoes']);
      });
    } else {
      this.cotacaoService.create(cotacao).subscribe(() => {
        this.router.navigate(['/cotacoes']);
      });
    }
  }
}
