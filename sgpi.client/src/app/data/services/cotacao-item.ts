import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CotacaoItem } from '@entities/cotacao-item';
import { environment } from '@env';

@Injectable({
  providedIn: 'root',
})
export class CotacaoItemService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/api/cotacoes`;

  getAll(): Observable<CotacaoItem[]> {
    return this.http.get<CotacaoItem[]>(this.apiUrl);
  }

  getById(id: number): Observable<CotacaoItem> {
    return this.http.get<CotacaoItem>(`${this.apiUrl}/${id}`);
  }

  getByItemCatalogoId(itemCatalogoId: number): Observable<CotacaoItem[]> {
    return this.http.get<CotacaoItem[]>(`${this.apiUrl}/item/${itemCatalogoId}`);
  }

  create(cotacao: CotacaoItem): Observable<CotacaoItem> {
    return this.http.post<CotacaoItem>(this.apiUrl, cotacao);
  }

  update(id: number, cotacao: CotacaoItem): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, cotacao);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
