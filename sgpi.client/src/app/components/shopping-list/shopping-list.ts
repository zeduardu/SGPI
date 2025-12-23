import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatTableModule } from '@angular/material/table';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatIconModule } from '@angular/material/icon';
import { Router } from '@angular/router';
import { EstoqueService } from '@services/estoque';
import { Estoque } from '@entities/estoque';
import { SelectionModel } from '@angular/cdk/collections';

@Component({
  selector: 'app-shopping-list',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatButtonModule,
    MatTableModule,
    MatCheckboxModule,
    MatIconModule,
  ],
  templateUrl: './shopping-list.html',
  styleUrl: './shopping-list.scss',
})
export class ShoppingListComponent implements OnInit {
  dataSource: Estoque[] = [];
  displayedColumns: string[] = [
    'select',
    'item',
    'estoqueAtual',
    'estoqueMinimo',
    'sugestaoCompra',
  ];
  selection = new SelectionModel<Estoque>(true, []);

  constructor(private estoqueService: EstoqueService, private router: Router) {}

  ngOnInit(): void {
    this.loadShoppingList();
  }

  loadShoppingList(): void {
    this.estoqueService.getShoppingList().subscribe({
      next: (data) => (this.dataSource = data),
      error: (err) => console.error('Error loading shopping list', err),
    });
  }

  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.dataSource.length;
    return numSelected === numRows;
  }

  toggleAllRows() {
    if (this.isAllSelected()) {
      this.selection.clear();
      return;
    }

    this.selection.select(...this.dataSource);
  }

  generateOrder() {
    if (this.selection.isEmpty()) return;

    const selectedItems = this.selection.selected.map((e) => ({
      itemCatalogoId: e.itemCatalogoId,
      quantidade: e.estoqueMaximo - e.quantidadeEmEstoque, // Suggestion: Fill up to max
      precoUnitario: 0, // Default or fetch last price
    }));

    // Navigate to Order Form with state
    this.router.navigate(['/ordem-compra/novo'], { state: { items: selectedItems } });
  }
}
