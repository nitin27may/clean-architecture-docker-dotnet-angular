import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { Role } from '@core/models/role.interface';
import { environment } from "@environments/environment";
import { User } from "../../../@core/models/user.interface";


@Injectable({
  providedIn: 'root'
})
export class UserRoleService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiEndpoint}/userroles`;

  getUserRoles(userId: string): Observable<User> {
    return this.http.get<User>(`${this.apiUrl}/${userId}`);
  }

  updateUserRoles(userId: string, roleIds: string[]): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/${userId}/roles`, { userId, roleIds });
  }
}