import { RolePermission } from './permission.interface';

export interface User {
    id: string;
    firstName: string;
    lastName: string;
    userName: string;
    mobile: string;
    email: string;
    rolePermissions: RolePermission[];
}
