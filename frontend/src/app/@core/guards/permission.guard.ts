import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { PermissionService } from '../services/permission.service';
import { MatSnackBar } from '@angular/material/snack-bar';

export const PermissionGuard = (pageName: string, operation: string = 'Read'): CanActivateFn => {
    return () => {
        const permissionService = inject(PermissionService);
        const router = inject(Router);
        const snackBar = inject(MatSnackBar);

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
