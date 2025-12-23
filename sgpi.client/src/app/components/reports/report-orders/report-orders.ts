import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatButtonModule } from '@angular/material/button';
import { MatTableModule } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { OrdemCompraService } from '@services/ordem-compra';
import { OrdemDeCompra, StatusOrdemDeCompra } from '@entities/ordem-compra';
import { UserService } from '@services/user';
import { Usuario } from '@entities/usuario';

@Component({
  selector: 'app-report-orders',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatButtonModule,
    MatTableModule,
    MatIconModule,
  ],
  templateUrl: './report-orders.html',
  styleUrl: './report-orders.scss',
})
export class ReportOrdersComponent implements OnInit {
  filterForm: FormGroup;
  dataSource: OrdemDeCompra[] = [];
  displayedColumns: string[] = ['id', 'data', 'fornecedor', 'status', 'solicitante', 'total'];

  users: Usuario[] = [];

  StatusOrdemDeCompra = StatusOrdemDeCompra;

  constructor(
    private fb: FormBuilder,
    private ordemCompraService: OrdemCompraService,
    private userService: UserService
  ) {
    this.filterForm = this.fb.group({
      start: [null],
      end: [null],
      status: [null],
      requesterId: [null],
    });
  }

  ngOnInit(): void {
    this.loadFiltersData();
  }

  loadFiltersData(): void {
    this.userService.getAll().subscribe((data) => (this.users = data));
  }

  search(): void {
    const filters = this.filterForm.value;
    this.ordemCompraService.getReport(filters).subscribe({
      next: (data) => (this.dataSource = data),
      error: (err) => console.error('Error loading report', err),
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

  calculateTotal(oc: OrdemDeCompra): number {
    return oc.itens.reduce((acc, item) => acc + item.quantidadeSolicitada * item.precoUnitario, 0);
  }

  exportCsv(): void {
    if (!this.dataSource.length) return;

    const header = ['ID', 'Data', 'Fornecedor', 'Status', 'Solicitante', 'Total'];
    const rows = this.dataSource.map((o) => [
      o.id,
      new Date(o.dataCriacao!).toLocaleDateString(),
      o.fornecedor?.nome || '',
      this.getStatusLabel(o.status),
      o.usuarioSolicitante?.nomeCompleto || '',
      this.calculateTotal(o).toFixed(2),
    ]);

    const csvContent = [header, ...rows].map((e) => e.join(',')).join('\n');
    const blob = new Blob([csvContent], { type: 'text/csv;charset=utf-8;' });
    const link = document.createElement('a');
    const url = URL.createObjectURL(blob);
    link.setAttribute('href', url);
    link.setAttribute('download', 'relatorio_ordens_compra.csv');
    link.style.visibility = 'hidden';
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
  }
}
