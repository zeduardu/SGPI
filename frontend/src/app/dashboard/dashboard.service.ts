import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { StockStatus } from '../interfaces/stock-status';
import { MovimentacaoEstoque } from '../interfaces/movimentacao-estoque';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class DashboardService {
  private apiUrl = `${environment.apiUrl}/api/dashboard`;

  constructor(private http: HttpClient) {}

  getStockStatus(): Observable<StockStatus[]> {
    return this.http.get<StockStatus[]>(`${this.apiUrl}/stock-status`);
  }

  getRecentMovements(): Observable<MovimentacaoEstoque[]> {
    return this.http.get<MovimentacaoEstoque[]>(`${this.apiUrl}/recent-movements`);
  }
}
