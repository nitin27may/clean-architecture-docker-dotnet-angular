import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { PermissionService } from '@core/services/permission.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AuthStateService } from '@core/services/auth-state.service';

export const PermissionGuard = (pageName: string, operation: string = 'Read'): CanActivateFn => {
    return () => {
        const permissionService = inject(PermissionService);
        const router = inject(Router);
        const snackBar = inject(MatSnackBar);
        const authState = inject(AuthStateService);

        // Initialize auth state if needed
        authState.initializeFromStorage();

        // Get current user state
        const currentUser = authState.getCurrentUser()();

        if (!currentUser) {
            snackBar.open('Please login to continue.', 'Close', {
                duration: 3000,
                horizontalPosition: 'end'
            });
            return router.parseUrl('/login');
        }

        if (permissionService.hasPermission(pageName, operation)) {
            return true;
        }

        snackBar.open('Access denied. You do not have permission to view this page.', 'Close', {
            duration: 3000,
            horizontalPosition: 'end'
        });

        return router.parseUrl('/');
    };
};
