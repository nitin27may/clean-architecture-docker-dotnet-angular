import { Routes } from "@angular/router";
import LayoutComponent from "@core/layout/layout.component";
import { redirectToLoginIfNotAuthenticated } from "@core/guards";
import { UserListComponent } from "./users/users-list/users-list.component";
import { PermissionGuard } from "@core/guards/permission.guard";
import { ActivityLogComponent } from "./activity-log/activity-log.component";

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
      }
    ]
  }
] as Routes;
