import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { RouterLink } from '@angular/router';
import { CategoriaService } from '../../../services/categoria';
import { Categoria } from '../../../interfaces/categoria';

@Component({
  selector: 'app-categoria-list',
  standalone: true,
  imports: [CommonModule, MatTableModule, MatButtonModule, MatIconModule, RouterLink],
  templateUrl: './categoria-list.html',
  styleUrls: ['./categoria-list.css'],
})
export class CategoriaListComponent implements OnInit {
  private categoriaService = inject(CategoriaService);
  categorias: Categoria[] = [];
  displayedColumns: string[] = ['id', 'nome', 'actions'];

  ngOnInit(): void {
    this.loadCategorias();
  }

  loadCategorias(): void {
    this.categoriaService.getAll().subscribe((data) => {
      this.categorias = data;
    });
  }

  deleteCategoria(id: number): void {
    if (confirm('Tem certeza que deseja excluir esta categoria?')) {
      this.categoriaService.delete(id).subscribe(() => {
        this.loadCategorias();
      });
    }
  }
}
