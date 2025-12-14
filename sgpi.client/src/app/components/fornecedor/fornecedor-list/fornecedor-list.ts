import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { RouterLink } from '@angular/router';
import { FornecedorService } from '../../../services/fornecedor';
import { Fornecedor } from '../../../interfaces/fornecedor';

@Component({
  selector: 'app-fornecedor-list',
  standalone: true,
  imports: [CommonModule, MatTableModule, MatButtonModule, MatIconModule, RouterLink],
  templateUrl: './fornecedor-list.html',
  styleUrls: ['./fornecedor-list.css'],
})
export class FornecedorListComponent implements OnInit {
  private fornecedorService = inject(FornecedorService);
  fornecedores: Fornecedor[] = [];
  displayedColumns: string[] = ['id', 'nome', 'email', 'telefone', 'cnpj', 'actions'];

  ngOnInit(): void {
    this.loadFornecedores();
  }

  loadFornecedores(): void {
    this.fornecedorService.getAll().subscribe((data) => {
      this.fornecedores = data;
    });
  }

  deleteFornecedor(id: number): void {
    if (confirm('Tem certeza que deseja excluir este fornecedor?')) {
      this.fornecedorService.delete(id).subscribe(() => {
        this.loadFornecedores();
      });
    }
  }
}
