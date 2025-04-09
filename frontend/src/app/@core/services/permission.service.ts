import { Injectable, inject } from '@angular/core';
import { AuthStateService } from './auth-state.service';
import { User } from '@core/models/user.interface';

@Injectable({
    providedIn: 'root'
})
export class PermissionService {
    private authState = inject(AuthStateService);

    hasPermission(page: string, operation: string): boolean {
        const user = this.authState.getCurrentUser()() as User;
        if (!user?.rolePermissions) return false;

        return user.rolePermissions.some(
            p => p.pageName.toLowerCase() === page.toLowerCase() &&
                 p.operationName.toLowerCase() === operation.toLowerCase()
        );
    }

    getPagePermissions(page: string): string[] {
        const user = this.authState.getCurrentUser()() as User;
        if (!user?.rolePermissions) return [];

        return user.rolePermissions
            .filter(p => p.pageName.toLowerCase() === page.toLowerCase())
            .map(p => p.operationName);
    }
}
