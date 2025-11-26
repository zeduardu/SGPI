import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { RouterModule } from '@angular/router';
import { OrdemCompraService } from '../../../services/ordem-compra';
import { OrdemDeCompra, StatusOrdemDeCompra } from '../../../interfaces/ordem-compra';

@Component({
  selector: 'app-ordem-compra-list',
  standalone: true,
  imports: [CommonModule, MatTableModule, MatButtonModule, MatIconModule, RouterModule],
  templateUrl: './ordem-compra-list.html',
  styleUrl: './ordem-compra-list.css',
})
export class OrdemCompraListComponent implements OnInit {
  displayedColumns: string[] = ['id', 'fornecedor', 'dataCriacao', 'status', 'acoes'];
  dataSource: OrdemDeCompra[] = [];

  constructor(private ordemCompraService: OrdemCompraService) {}

  ngOnInit(): void {
    this.loadOrdensCompra();
  }

  loadOrdensCompra(): void {
    this.ordemCompraService.getAll().subscribe({
      next: (data) => {
        this.dataSource = data;
      },
      error: (err) => {
        console.error('Error fetching OCs', err);
      },
    });
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
