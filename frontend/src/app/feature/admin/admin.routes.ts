import { Routes } from "@angular/router";
import LayoutComponent from "@core/layout/layout.component";
import { redirectToLoginIfNotAuthenticated } from "@core/guards";
import { PermissionGuard } from "@core/guards/permission.guard";
import { ActivityLogComponent } from "./activity-log/activity-log.component";
import { PagesComponent } from "./pages/pages.component";
import { OperationsComponent } from "./operations/operations.component";
import { UserRolesComponent } from "./user-roles/user-roles.component";
import { RolesComponent } from "./roles/roles.component";
import { UsersComponent } from "./users/users.component";
import { RolePermissionMappingComponent } from "./role-permission-mapping/role-permission-mapping.component";

export default [
  {
    path: '',
    component: LayoutComponent,
    canActivate: [redirectToLoginIfNotAuthenticated()],
    children: [
      {
        path: 'users',
        component: UsersComponent,
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
        path: 'user-roles',
        component: UserRolesComponent,
        canActivate: [PermissionGuard('UserRoles', 'Read')]
      },
      {
        path: 'role-permission-mapping',
        component: RolePermissionMappingComponent,
        canActivate: [PermissionGuard('RolePermissionMapping', 'Read')]
      },
      {
        path: '',
        redirectTo: 'users',
        pathMatch: 'full'
      }
    ]
  }
] as Routes;
