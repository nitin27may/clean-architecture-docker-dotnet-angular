import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { Page } from '@core/models/page.interface';
import { environment } from "@environments/environment";

@Injectable({
  providedIn: 'root'
})
export class PageService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiEndpoint}/pages`;

  getPages(): Observable<Page[]> {
    return this.http.get<Page[]>(this.apiUrl);
  }

  getPage(id: string): Observable<Page> {
    return this.http.get<Page>(`${this.apiUrl}/${id}`);
  }

  createPage(page: Partial<Page>): Observable<Page> {
    return this.http.post<Page>(this.apiUrl, page);
  }

  updatePage(id: string, page: Partial<Page>): Observable<Page> {
    return this.http.put<Page>(`${this.apiUrl}/${id}`, page);
  }

  deletePage(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}