import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { RouterLink } from '@angular/router';
import { ItemCatalogoService } from '@services/item-catalogo';
import { ItemCatalogo, UnidadeMedida } from '@entities/item-catalogo';

@Component({
  selector: 'app-item-catalogo-list',
  standalone: true,
  imports: [CommonModule, MatTableModule, MatButtonModule, MatIconModule, RouterLink],
  templateUrl: './item-catalogo-list.html',
  styleUrls: ['./item-catalogo-list.scss'],
})
export class ItemCatalogoListComponent implements OnInit {
  private itemService = inject(ItemCatalogoService);
  itens: ItemCatalogo[] = [];
  displayedColumns: string[] = ['id', 'nome', 'unidade', 'categoria', 'actions'];
  UnidadeMedida = UnidadeMedida;

  ngOnInit(): void {
    this.loadItens();
  }

  loadItens(): void {
    this.itemService.getAll().subscribe((data) => {
      this.itens = data;
    });
  }

  deleteItem(id: number): void {
    if (confirm('Tem certeza que deseja excluir este item?')) {
      this.itemService.delete(id).subscribe(() => {
        this.loadItens();
      });
    }
  }

  getUnidadeLabel(unidade: UnidadeMedida): string {
    switch (unidade) {
      case UnidadeMedida.UN:
        return 'UN';
      case UnidadeMedida.KIT:
        return 'KIT';
      case UnidadeMedida.METRO:
        return 'METRO';
      default:
        return '';
    }
  }
}
