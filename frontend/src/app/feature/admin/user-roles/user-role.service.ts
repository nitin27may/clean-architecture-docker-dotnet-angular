import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { Role } from '@core/models/role.interface';
import { environment } from "@environments/environment";


@Injectable({
  providedIn: 'root'
})
export class UserRoleService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiEndpoint}/userroles`;

  getUserRoles(userId: string): Observable<Role[]> {
    return this.http.get<Role[]>(`${this.apiUrl}/${userId}`);
  }

  updateUserRoles(userId: string, roleIds: string[]): Observable<any> {
    return this.http.post<any>(this.apiUrl, { userId, roleIds });
  }
}