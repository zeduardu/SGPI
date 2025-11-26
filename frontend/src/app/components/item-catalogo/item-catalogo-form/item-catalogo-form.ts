import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatSelectModule } from '@angular/material/select';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { ItemCatalogoService } from '../../../services/item-catalogo';
import { CategoriaService } from '../../../services/categoria';
import { ItemCatalogo, UnidadeMedida } from '../../../interfaces/item-catalogo';
import { Categoria } from '../../../interfaces/categoria';

@Component({
  selector: 'app-item-catalogo-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatInputModule,
    MatButtonModule,
    MatCardModule,
    MatSelectModule,
    RouterLink,
  ],
  templateUrl: './item-catalogo-form.html',
  styleUrls: ['./item-catalogo-form.css'],
})
export class ItemCatalogoFormComponent implements OnInit {
  private fb = inject(FormBuilder);
  private itemService = inject(ItemCatalogoService);
  private categoriaService = inject(CategoriaService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  form: FormGroup = this.fb.group({
    nome: ['', [Validators.required, Validators.minLength(3)]],
    descricaoDetalhada: [''],
    unidadeMedida: [UnidadeMedida.UN, Validators.required],
    categoriaId: [null, Validators.required],
  });

  isEditMode = false;
  itemId?: number;
  categorias: Categoria[] = [];
  unidades = [
    { value: UnidadeMedida.UN, label: 'UN' },
    { value: UnidadeMedida.KIT, label: 'KIT' },
    { value: UnidadeMedida.METRO, label: 'METRO' },
  ];

  ngOnInit(): void {
    this.loadCategorias();
    const id = this.route.snapshot.paramMap.get('id');
    if (id && id !== 'new') {
      this.isEditMode = true;
      this.itemId = +id;
      this.loadItem(this.itemId);
    }
  }

  loadCategorias(): void {
    this.categoriaService.getAll().subscribe((data) => {
      this.categorias = data;
    });
  }

  loadItem(id: number): void {
    this.itemService.getById(id).subscribe((item) => {
      this.form.patchValue(item);
    });
  }

  save(): void {
    if (this.form.invalid) return;

    const item: ItemCatalogo = {
      id: this.itemId || 0,
      ...this.form.value,
    };

    if (this.isEditMode) {
      this.itemService.update(this.itemId!, item).subscribe(() => {
        this.router.navigate(['/itens-catalogo']);
      });
    } else {
      this.itemService.create(item).subscribe(() => {
        this.router.navigate(['/itens-catalogo']);
      });
    }
  }
}
