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
import { MovimentacaoService } from '@services/movimentacao';
import { MovimentacaoEstoque, TipoMovimentacao } from '@entities/movimentacao-estoque';
import { ItemCatalogoService } from '@services/item-catalogo';
import { UserService } from '@services/user';
import { ItemCatalogo } from '@entities/item-catalogo';
import { Usuario } from '@entities/usuario';

@Component({
  selector: 'app-report-movements',
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
  templateUrl: './report-movements.html',
  styleUrl: './report-movements.scss',
})
export class ReportMovementsComponent implements OnInit {
  filterForm: FormGroup;
  dataSource: MovimentacaoEstoque[] = [];
  displayedColumns: string[] = ['data', 'tipo', 'item', 'quantidade', 'usuario', 'valor'];

  itens: ItemCatalogo[] = [];
  users: Usuario[] = [];

  TipoMovimentacao = TipoMovimentacao;

  constructor(
    private fb: FormBuilder,
    private movimentacaoService: MovimentacaoService,
    private itemService: ItemCatalogoService,
    private userService: UserService
  ) {
    this.filterForm = this.fb.group({
      start: [null],
      end: [null],
      type: [null],
      itemId: [null],
      userId: [null],
    });
  }

  ngOnInit(): void {
    this.loadFiltersData();
  }

  loadFiltersData(): void {
    this.itemService.getAll().subscribe((data) => (this.itens = data));
    this.userService.getAll().subscribe((data) => (this.users = data));
  }

  search(): void {
    const filters = this.filterForm.value;
    this.movimentacaoService.getReport(filters).subscribe({
      next: (data) => (this.dataSource = data),
      error: (err) => console.error('Error loading report', err),
    });
  }

  getTipoLabel(tipo: TipoMovimentacao): string {
    switch (tipo) {
      case TipoMovimentacao.ENTRADA:
        return 'ENTRADA';
      case TipoMovimentacao.SAIDA:
        return 'SAÍDA';
      case TipoMovimentacao.AJUSTE_INVENTARIO:
        return 'AJUSTE';
      default:
        return 'DESCONHECIDO';
    }
  }

  exportCsv(): void {
    // Basic CSV export implementation
    if (!this.dataSource.length) return;

    const header = ['Data', 'Tipo', 'Item', 'Quantidade', 'Usuário', 'Valor Unitário'];
    const rows = this.dataSource.map((m) => [
      new Date(m.data).toLocaleDateString(),
      this.getTipoLabel(m.tipoMovimentacao),
      m.itemCatalogo?.nome || '',
      m.quantidade,
      m.usuario?.nomeCompleto || '',
      m.valorUnitarioEfetivo || '',
    ]);

    const csvContent = [header, ...rows].map((e) => e.join(',')).join('\n');
    const blob = new Blob([csvContent], { type: 'text/csv;charset=utf-8;' });
    const link = document.createElement('a');
    const url = URL.createObjectURL(blob);
    link.setAttribute('href', url);
    link.setAttribute('download', 'relatorio_movimentacoes.csv');
    link.style.visibility = 'hidden';
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
  }
}
