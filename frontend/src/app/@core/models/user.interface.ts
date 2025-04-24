import { RolePermission } from './permission.interface';
import { Role } from "./role.interface";

export interface User {
    id: string;
    firstName: string;
    lastName: string;
    userName: string;
    mobile: string;
    email: string;
    rolePermissions?: RolePermission[];
    roles?: Role[];
}
