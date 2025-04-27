import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '@environments/environment';
import { 
  RolePermissionMappingResponse, 
  RolePermissionMappingRequest, 
  RoleModel 
} from '@core/models/role-permission-mapping.model';

@Injectable({
  providedIn: 'root'
})
export class RolePermissionMappingService {
  private http = inject(HttpClient);
  private baseUrl = `${environment.apiEndpoint}/RolePermissionMapping`;
  
  /**
   * Get all available roles for selection
   */
  getRoles(): Observable<RoleModel[]> {
    return this.http.get<RoleModel[]>(`${environment.apiEndpoint}/roles`);
  }
  
  /**
   * Get role-permission mapping for a specific role
   * @param roleId The ID of the role to get mapping for
   */
  getRolePermissionMapping(roleId: string): Observable<RolePermissionMappingResponse> {
    return this.http.get<RolePermissionMappingResponse>(`${this.baseUrl}/${roleId}`);
  }
  
  /**
   * Save updated role-permission mapping
   * @param mapping The updated role-permission mapping to save
   */
  saveRolePermissionMapping(mapping: RolePermissionMappingRequest): Observable<any> {
    return this.http.post<any>(this.baseUrl, mapping);
  }
}