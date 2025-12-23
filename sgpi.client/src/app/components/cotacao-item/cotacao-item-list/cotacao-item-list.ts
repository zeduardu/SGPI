import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { RouterLink } from '@angular/router';
import { CotacaoItemService } from '@services/cotacao-item';
import { CotacaoItem } from '@entities/cotacao-item';

@Component({
  selector: 'app-cotacao-item-list',
  standalone: true,
  imports: [CommonModule, MatTableModule, MatButtonModule, MatIconModule, RouterLink],
  templateUrl: './cotacao-item-list.html',
  styleUrls: ['./cotacao-item-list.scss'],
})
export class CotacaoItemListComponent implements OnInit {
  private cotacaoService = inject(CotacaoItemService);
  cotacoes: CotacaoItem[] = [];
  displayedColumns: string[] = ['item', 'fornecedor', 'preco', 'data', 'link', 'actions'];

  ngOnInit(): void {
    this.loadCotacoes();
  }

  loadCotacoes(): void {
    this.cotacaoService.getAll().subscribe((data) => {
      this.cotacoes = data;
    });
  }

  deleteCotacao(id: number): void {
    if (confirm('Tem certeza que deseja excluir esta cotação?')) {
      this.cotacaoService.delete(id).subscribe(() => {
        this.loadCotacoes();
      });
    }
  }
}
