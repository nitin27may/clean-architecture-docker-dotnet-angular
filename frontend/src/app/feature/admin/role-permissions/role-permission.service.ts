import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { Permission, RolePermission } from '@core/models/permission.interface';
import { environment } from "@environments/environment";


@Injectable({
  providedIn: 'root'
})
export class RolePermissionService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiEndpoint}/rolepermissions`;

  getRolePermissions(roleId: string): Observable<Permission[]> {
    return this.http.get<Permission[]>(`${this.apiUrl}/${roleId}`);
  }

  assignPermissionsToRole(data: { roleId: string, permissionIds: string[] }): Observable<any> {
    return this.http.post<any>(this.apiUrl, data);
  }

  removeRolePermission(roleId: string, permissionId: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${roleId}/${permissionId}`);
  }

  getAllRolePermissions(): Observable<RolePermission[]> {
    return this.http.get<RolePermission[]>(`${this.apiUrl}`);
  }
}