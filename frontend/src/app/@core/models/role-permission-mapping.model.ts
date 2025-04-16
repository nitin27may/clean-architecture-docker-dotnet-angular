// Models for role-permission mapping
export interface RolePermissionMappingResponse {
  roleId: string;
  roleName: string;
  pages: PageOperationResponse[];
}

export interface PageOperationResponse {
  pageId: string;
  pageName: string;
  operations: OperationResponse[];
}

export interface OperationResponse {
  operationId: string;
  operationName: string;
  isSelected: boolean;
}

export interface RolePermissionMappingRequest {
  roleId: string;
  permissions: PageOperationRequest[];
}

export interface PageOperationRequest {
  pageId: string;
  operationIds: string[];
}

// Simple role model for dropdown selection
export interface RoleModel {
  id: string;
  name: string;
}