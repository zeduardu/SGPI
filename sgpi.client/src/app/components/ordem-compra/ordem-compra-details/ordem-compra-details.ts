import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatTableModule } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { OrdemCompraService } from '@services/ordem-compra';
import { OrdemDeCompra, StatusOrdemDeCompra } from '../../../data/types/ordem-compra';
import { FormBuilder, FormGroup, FormArray, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';

@Component({
  selector: 'app-ordem-compra-details',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatButtonModule,
    MatTableModule,
    MatIconModule,
    RouterModule,
    ReactiveFormsModule,
    MatInputModule,
    MatFormFieldModule,
  ],
  templateUrl: './ordem-compra-details.html',
  styleUrl: './ordem-compra-details.scss',
})
export class OrdemCompraDetailsComponent implements OnInit {
  ordemCompra?: OrdemDeCompra;
  displayedColumns: string[] = ['item', 'quantidade', 'recebido', 'precoUnitario', 'total'];
  StatusOrdemDeCompra = StatusOrdemDeCompra;

  isReceiving = false;
  receiveForm: FormGroup;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private ordemCompraService: OrdemCompraService,
    private fb: FormBuilder
  ) {
    this.receiveForm = this.fb.group({
      itens: this.fb.array([]),
    });
  }

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    if (id) {
      this.loadOrdemCompra(id);
    }
  }

  get itensFormArray(): FormArray {
    return this.receiveForm.get('itens') as FormArray;
  }

  loadOrdemCompra(id: number): void {
    this.ordemCompraService.getById(id).subscribe({
      next: (data) => {
        this.ordemCompra = data;
        this.initReceiveForm();
      },
      error: (err) => {
        console.error('Error fetching OC details', err);
      },
    });
  }

  initReceiveForm(): void {
    this.itensFormArray.clear();
    if (this.ordemCompra) {
      this.ordemCompra.itens.forEach((item) => {
        const remaining = item.quantidadeSolicitada - item.quantidadeRecebida;
        this.itensFormArray.push(
          this.fb.group({
            itemCatalogoId: [item.itemCatalogoId],
            quantidade: [
              remaining > 0 ? remaining : 0,
              [Validators.required, Validators.min(0), Validators.max(remaining)],
            ],
            valorUnitario: [item.precoUnitario, [Validators.required, Validators.min(0)]],
          })
        );
      });
    }
  }

  toggleReceiving(): void {
    this.isReceiving = !this.isReceiving;
    if (this.isReceiving) {
      this.displayedColumns = [
        'item',
        'quantidade',
        'recebido',
        'aReceber',
        'valorRecebimento',
        'total',
      ];
    } else {
      this.displayedColumns = ['item', 'quantidade', 'recebido', 'precoUnitario', 'total'];
    }
  }

  approve(): void {
    if (this.ordemCompra) {
      this.ordemCompraService.approve(this.ordemCompra.id!).subscribe({
        next: () => this.loadOrdemCompra(this.ordemCompra!.id!),
        error: (err) => console.error('Error approving OC', err),
      });
    }
  }

  submitReceive(): void {
    if (this.receiveForm.invalid || !this.ordemCompra) return;

    const itensToReceive = this.itensFormArray.value
      .filter((item: any) => item.quantidade > 0)
      .map((item: any) => ({
        itemCatalogoId: item.itemCatalogoId,
        quantidade: item.quantidade,
        valorUnitario: item.valorUnitario,
      }));

    if (itensToReceive.length === 0) return;

    this.ordemCompraService.receive(this.ordemCompra.id!, itensToReceive).subscribe({
      next: () => {
        this.isReceiving = false;
        this.loadOrdemCompra(this.ordemCompra!.id!);
      },
      error: (err) => console.error('Error receiving OC', err),
    });
  }

  cancel(): void {
    if (this.ordemCompra) {
      this.ordemCompraService.cancel(this.ordemCompra.id!).subscribe({
        next: () => this.loadOrdemCompra(this.ordemCompra!.id!),
        error: (err) => console.error('Error canceling OC', err),
      });
    }
  }

  getStatusLabel(status: StatusOrdemDeCompra): string {
    switch (status) {
      case StatusOrdemDeCompra.ABERTA:
        return 'ABERTA';
      case StatusOrdemDeCompra.APROVADA:
        return 'APROVADA';
      case StatusOrdemDeCompra.CONCLUIDA_PARCIAL:
        return 'CONCLUÍDA PARCIAL';
      case StatusOrdemDeCompra.CONCLUIDA_TOTAL:
        return 'CONCLUÍDA TOTAL';
      case StatusOrdemDeCompra.CANCELADA:
        return 'CANCELADA';
      case StatusOrdemDeCompra.REJEITADA:
        return 'REJEITADA';
      default:
        return 'DESCONHECIDO';
    }
  }
}
