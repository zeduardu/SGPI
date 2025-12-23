import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { OrdemDeCompra, ItemRecebimentoDto } from '@entities/ordem-compra';
import { environment } from '@env';

@Injectable({
  providedIn: 'root',
})
export class OrdemCompraService {
  private apiUrl = `${environment.apiUrl}/ordens-compra`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<OrdemDeCompra[]> {
    return this.http.get<OrdemDeCompra[]>(this.apiUrl);
  }

  getById(id: number): Observable<OrdemDeCompra> {
    return this.http.get<OrdemDeCompra>(`${this.apiUrl}/${id}`);
  }

  create(ordemCompra: Partial<OrdemDeCompra>): Observable<OrdemDeCompra> {
    return this.http.post<OrdemDeCompra>(this.apiUrl, ordemCompra);
  }

  approve(id: number): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/${id}/approve`, {});
  }

  receive(id: number, itens: ItemRecebimentoDto[]): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/${id}/receive`, itens);
  }

  cancel(id: number): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/${id}/cancel`, {});
  }

  getReport(filters: any): Observable<OrdemDeCompra[]> {
    let params = new HttpParams();
    if (filters.start) params = params.set('start', filters.start.toISOString());
    if (filters.end) params = params.set('end', filters.end.toISOString());
    if (filters.status !== undefined && filters.status !== null)
      params = params.set('status', filters.status);
    if (filters.requesterId) params = params.set('requesterId', filters.requesterId);

    return this.http.get<OrdemDeCompra[]>(`${environment.apiUrl}/reports/orders`, { params });
  }
}
