import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatCardModule } from '@angular/material/card';
import { HttpClientModule } from '@angular/common/http';
import { Chart } from 'chart.js/auto';
import { DashboardService } from './dashboard.service';
import { StockStatus } from '../data/types/stock-status';
import { MovimentacaoEstoque } from '../data/types/movimentacao-estoque';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, MatTableModule, MatCardModule, HttpClientModule],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.scss',
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
  chart!: Chart;

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
        this.createChart();
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

  createChart(): void {
    if (this.chart) {
      this.chart.destroy();
    }

    const statusCounts = this.dataSource.reduce(
      (acc, item) => {
        if (item.status === 'OK') {
          acc.ok++;
        } else if (item.status === 'ATENÇÃO') {
          acc.atencao++;
        } else if (item.status === 'COMPRAR') {
          acc.comprar++;
        }
        return acc;
      },
      { ok: 0, atencao: 0, comprar: 0 }
    );

    this.chart = new Chart('stockStatusChart', {
      type: 'doughnut',
      data: {
        labels: ['OK', 'ATENÇÃO', 'COMPRAR'],
        datasets: [
          {
            label: 'Status do Estoque',
            data: [statusCounts.ok, statusCounts.atencao, statusCounts.comprar],
            backgroundColor: ['#4CAF50', '#FFC107', '#F44336'],
          },
        ],
      },
    });
  }
}
