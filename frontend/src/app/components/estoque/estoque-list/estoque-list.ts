import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { RouterLink } from '@angular/router';
import { EstoqueService } from '../../../services/estoque';
import { Estoque } from '../../../interfaces/estoque';

@Component({
  selector: 'app-estoque-list',
  standalone: true,
  imports: [CommonModule, MatTableModule, MatButtonModule, MatIconModule, RouterLink],
  templateUrl: './estoque-list.html',
  styleUrls: ['./estoque-list.css'],
})
export class EstoqueListComponent implements OnInit {
  private estoqueService = inject(EstoqueService);
  estoques: Estoque[] = [];
  displayedColumns: string[] = ['item', 'min', 'max', 'atual', 'actions'];

  ngOnInit(): void {
    this.loadEstoque();
  }

  loadEstoque(): void {
    this.estoqueService.getAll().subscribe((data) => {
      this.estoques = data;
    });
  }

  deleteEstoque(id: number): void {
    if (confirm('Tem certeza que deseja excluir este registro de estoque?')) {
      this.estoqueService.delete(id).subscribe(() => {
        this.loadEstoque();
      });
    }
  }
}
