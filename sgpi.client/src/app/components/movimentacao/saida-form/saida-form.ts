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
  selector: 'app-saida-form',
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
  templateUrl: './saida-form.html',
  styleUrls: ['./saida-form.scss'],
})
export class SaidaFormComponent implements OnInit {
  private fb = inject(FormBuilder);
  private movimentacaoService = inject(MovimentacaoService);
  private itemService = inject(ItemCatalogoService);
  private router = inject(Router);

  form: FormGroup = this.fb.group({
    itemCatalogoId: [null, Validators.required],
    quantidade: [1, [Validators.required, Validators.min(1)]],
    solicitante: ['', Validators.required],
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

    this.movimentacaoService.registrarSaida(this.form.value).subscribe({
      next: () => {
        alert('Saída registrada com sucesso!');
        this.router.navigate(['/dashboard']); // Or back to stock list
      },
      error: (err) => {
        alert('Erro ao registrar saída: ' + (err.error || err.message));
      },
    });
  }
}
