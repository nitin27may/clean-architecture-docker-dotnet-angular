import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { Operation } from '@core/models/operation.interface';
import { environment } from "@environments/environment";

@Injectable({
  providedIn: 'root'
})
export class OperationService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiEndpoint}/operations`;

  getOperations(): Observable<Operation[]> {
    return this.http.get<Operation[]>(this.apiUrl);
  }

  getOperation(id: string): Observable<Operation> {
    return this.http.get<Operation>(`${this.apiUrl}/${id}`);
  }

  createOperation(operation: Partial<Operation>): Observable<Operation> {
    return this.http.post<Operation>(this.apiUrl, operation);
  }

  updateOperation(id: string, operation: Partial<Operation>): Observable<Operation> {
    return this.http.put<Operation>(`${this.apiUrl}/${id}`, operation);
  }

  deleteOperation(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}