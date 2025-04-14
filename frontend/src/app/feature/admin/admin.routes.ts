import { Routes } from "@angular/router";
import LayoutComponent from "@core/layout/layout.component";
import { redirectToLoginIfNotAuthenticated } from "@core/guards";
import { UserListComponent } from "./users/users-list/users-list.component";
import { PermissionGuard } from "@core/guards/permission.guard";
import { ActivityLogComponent } from "./activity-log/activity-log.component";
import { PagesComponent } from "./pages/pages.component";
import { OperationsComponent } from "./operations/operations.component";
import { PermissionsComponent } from "./permissions/permissions.component";
import { RolePermissionsComponent } from "./role-permissions/role-permissions.component";
import { UserRolesComponent } from "./user-roles/user-roles.component";
import { RolesComponent } from "./roles/roles.component";

export default [
  {
    path: '',
    component: LayoutComponent,
    canActivate: [redirectToLoginIfNotAuthenticated()],
    children: [
      {
        path: 'users',
        component: UserListComponent,
        canActivate: [PermissionGuard('Users', 'Read')]
      },
      {
        path: 'activity-logs',
        component: ActivityLogComponent,
        canActivate: [PermissionGuard('ActivityLog', 'Read')]
      },
      {
        path: 'pages',
        component: PagesComponent,
        canActivate: [PermissionGuard('Pages', 'Read')]
      },
      {
        path: 'operations',
        component: OperationsComponent,
        canActivate: [PermissionGuard('Operations', 'Read')]
      },
      {
        path: 'roles',
        component: RolesComponent,
        canActivate: [PermissionGuard('Roles', 'Read')]
      },
      {
        path: 'permissions',
        component: PermissionsComponent,
        canActivate: [PermissionGuard('Permissions', 'Read')]
      },
      {
        path: 'role-permissions',
        component: RolePermissionsComponent,
        canActivate: [PermissionGuard('RolePermissions', 'Read')]
      },
      {
        path: 'user-roles',
        component: UserRolesComponent,
        canActivate: [PermissionGuard('UserRoles', 'Read')]
      },
      {
        path: '',
        redirectTo: 'users',
        pathMatch: 'full'
      }
    ]
  }
] as Routes;
