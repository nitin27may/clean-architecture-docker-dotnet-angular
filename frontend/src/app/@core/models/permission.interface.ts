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
    roleName: string;
    pageName: string;
    pageUrl: string;
    operationName: string;
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
