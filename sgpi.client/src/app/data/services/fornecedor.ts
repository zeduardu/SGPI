import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Fornecedor } from '@entities/fornecedor';
import { environment } from '@env';

@Injectable({
  providedIn: 'root',
})
export class FornecedorService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/api/fornecedores`;

  getAll(): Observable<Fornecedor[]> {
    return this.http.get<Fornecedor[]>(this.apiUrl);
  }

  getById(id: number): Observable<Fornecedor> {
    return this.http.get<Fornecedor>(`${this.apiUrl}/${id}`);
  }

  create(fornecedor: Fornecedor): Observable<Fornecedor> {
    return this.http.post<Fornecedor>(this.apiUrl, fornecedor);
  }

  update(id: number, fornecedor: Fornecedor): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, fornecedor);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
