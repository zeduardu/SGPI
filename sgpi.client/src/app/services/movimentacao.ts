import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import {
  MovimentacaoEstoque,
  RegistrarSaidaRequest,
  RegistrarAjusteRequest,
} from '../interfaces/movimentacao-estoque';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class MovimentacaoService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/api/movimentacoes`;

  getAll(): Observable<MovimentacaoEstoque[]> {
    return this.http.get<MovimentacaoEstoque[]>(this.apiUrl);
  }

  getByItemCatalogoId(itemCatalogoId: number): Observable<MovimentacaoEstoque[]> {
    return this.http.get<MovimentacaoEstoque[]>(`${this.apiUrl}/item/${itemCatalogoId}`);
  }

  registrarSaida(request: RegistrarSaidaRequest): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/saida`, request);
  }

  registrarAjuste(request: RegistrarAjusteRequest): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/ajuste`, request);
  }

  getReport(filters: any): Observable<MovimentacaoEstoque[]> {
    let params = new HttpParams();
    if (filters.start) params = params.set('start', filters.start.toISOString());
    if (filters.end) params = params.set('end', filters.end.toISOString());
    if (filters.type) params = params.set('type', filters.type);
    if (filters.userId) params = params.set('userId', filters.userId);
    if (filters.itemId) params = params.set('itemId', filters.itemId);

    return this.http.get<MovimentacaoEstoque[]>(`${environment.apiUrl}/reports/movements`, {
      params,
    });
  }
}
