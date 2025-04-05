export interface RolePermission {
    roleName: string;
    pageName: string;
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