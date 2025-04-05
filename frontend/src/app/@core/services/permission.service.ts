import { Injectable, inject } from '@angular/core';
import { AuthStateService } from './auth-state.service';
import { User } from '../models/user.interface';

@Injectable({
    providedIn: 'root'
})
export class PermissionService {
    private authState = inject(AuthStateService);

    hasPermission(pageName: string, operationName: string): boolean {
        const user = this.authState.getCurrentUser()() as User;
        if (!user?.rolePermissions) return false;

        return user.rolePermissions.some(
            permission =>
                permission.pageName === pageName &&
                permission.operationName === operationName
        );
    }

    getPagePermissions(pageName: string): string[] {
        const user = this.authState.getCurrentUser()() as User;
        if (!user?.rolePermissions) return [];

        return user.rolePermissions
            .filter(permission => permission.pageName === pageName)
            .map(permission => permission.operationName);
    }
}
