import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Estoque } from '@entities/estoque';
import { environment } from '@env';

@Injectable({
  providedIn: 'root',
})
export class EstoqueService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/api/estoque`;

  getAll(): Observable<Estoque[]> {
    return this.http.get<Estoque[]>(this.apiUrl);
  }

  getById(id: number): Observable<Estoque> {
    return this.http.get<Estoque>(`${this.apiUrl}/${id}`);
  }

  getByItemCatalogoId(itemCatalogoId: number): Observable<Estoque> {
    return this.http.get<Estoque>(`${this.apiUrl}/item/${itemCatalogoId}`);
  }

  create(estoque: Estoque): Observable<Estoque> {
    return this.http.post<Estoque>(this.apiUrl, estoque);
  }

  update(id: number, estoque: Estoque): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, estoque);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  getShoppingList(): Observable<Estoque[]> {
    return this.http.get<Estoque[]>(`${this.apiUrl}/shopping-list`);
  }
}
