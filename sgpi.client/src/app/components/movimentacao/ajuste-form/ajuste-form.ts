import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatSelectModule } from '@angular/material/select';
import { Router, RouterLink } from '@angular/router';
import { MovimentacaoService } from '@services/movimentacao';
import { ItemCatalogoService } from '@services/item-catalogo';
import { ItemCatalogo } from '@entities/item-catalogo';

@Component({
  selector: 'app-ajuste-form',
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
  templateUrl: './ajuste-form.html',
  styleUrls: ['./ajuste-form.scss'],
})
export class AjusteFormComponent implements OnInit {
  private fb = inject(FormBuilder);
  private movimentacaoService = inject(MovimentacaoService);
  private itemService = inject(ItemCatalogoService);
  private router = inject(Router);

  form: FormGroup = this.fb.group({
    itemCatalogoId: [null, Validators.required],
    novaQuantidade: [0, [Validators.required, Validators.min(0)]],
    observacao: ['', Validators.required],
  });

  itens: ItemCatalogo[] = [];

  ngOnInit(): void {
    this.loadItens();
  }

  loadItens(): void {
    this.itemService.getAll().subscribe((data) => {
      this.itens = data;
    });
  }

  save(): void {
    if (this.form.invalid) return;

    this.movimentacaoService.registrarAjuste(this.form.value).subscribe({
      next: () => {
        alert('Ajuste de inventário realizado com sucesso!');
        this.router.navigate(['/dashboard']);
      },
      error: (err) => {
        alert('Erro ao realizar ajuste: ' + (err.error || err.message));
      },
    });
  }
}
