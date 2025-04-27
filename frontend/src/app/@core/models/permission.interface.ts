export interface Permission {
  id: string;
  pageId: string;
  operationId: string;
  description?: string | null;
  pageName?: string;
  operationName?: string;
  createdOn: string;
  createdBy: string;
  updatedOn?: string | null;
  updatedBy?: string | null;
}

export interface RolePermission {
    roleId: string;
    roleName: string;
    pageId: string;
    pageName: string;
    pageUrl: string;

    pageOrder?: number; // Added for menu ordering
    operationId: string;
    operationName: string;
    permissionId: string;
}

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

export interface UserWithPermissions {
    id: string;
    firstName: string;
    lastName: string;
    userName: string;
    mobile: string;
    email: string;
    rolePermissions: RolePermission[];
}
