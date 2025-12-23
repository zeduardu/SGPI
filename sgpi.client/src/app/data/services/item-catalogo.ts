import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ItemCatalogo } from '@entities/item-catalogo';
import { environment } from '@env';

@Injectable({
  providedIn: 'root',
})
export class ItemCatalogoService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/api/itens-catalogo`;

  getAll(): Observable<ItemCatalogo[]> {
    return this.http.get<ItemCatalogo[]>(this.apiUrl);
  }

  getById(id: number): Observable<ItemCatalogo> {
    return this.http.get<ItemCatalogo>(`${this.apiUrl}/${id}`);
  }

  create(item: ItemCatalogo): Observable<ItemCatalogo> {
    return this.http.post<ItemCatalogo>(this.apiUrl, item);
  }

  update(id: number, item: ItemCatalogo): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, item);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
