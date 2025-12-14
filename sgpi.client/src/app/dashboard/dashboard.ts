import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatCardModule } from '@angular/material/card';
import { HttpClientModule } from '@angular/common/http';
import { DashboardService } from './dashboard.service';
import { StockStatus } from '../interfaces/stock-status';
import { MovimentacaoEstoque } from '../interfaces/movimentacao-estoque';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, MatTableModule, MatCardModule, HttpClientModule],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css',
  providers: [DashboardService],
})
export class Dashboard implements OnInit {
  displayedColumns: string[] = [
    'item',
    'categoria',
    'quantidadeEmEstoque',
    'estoqueMinimo',
    'status',
  ];
  recentMovementsColumns: string[] = ['data', 'item', 'tipo', 'quantidade', 'usuario'];
  dataSource: StockStatus[] = [];
  recentMovements: MovimentacaoEstoque[] = [];

  constructor(private dashboardService: DashboardService) {}

  ngOnInit(): void {
    this.loadStockStatus();
    this.loadRecentMovements();
  }

  loadStockStatus(): void {
    this.dashboardService.getStockStatus().subscribe({
      next: (data) => {
        this.dataSource = data.map((item) => ({
          ...item,
          status: this.calculateStatus(item.quantidadeEmEstoque, item.estoqueMinimo),
        }));
      },
      error: (err) => {
        console.error('Error fetching stock status', err);
      },
    });
  }

  loadRecentMovements(): void {
    this.dashboardService.getRecentMovements().subscribe({
      next: (data) => {
        this.recentMovements = data;
      },
      error: (err) => {
        console.error('Error fetching recent movements', err);
      },
    });
  }

  calculateStatus(quantidadeEmEstoque: number, estoqueMinimo: number): string {
    if (quantidadeEmEstoque <= estoqueMinimo) {
      return 'COMPRAR';
    } else if (quantidadeEmEstoque > estoqueMinimo && quantidadeEmEstoque < estoqueMinimo * 1.2) {
      return 'ATENÇÃO';
    } else {
      return 'OK';
    }
  }
}
