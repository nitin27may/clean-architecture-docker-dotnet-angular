import { Routes } from "@angular/router";
import LayoutComponent from "@core/layout/layout.component";
import { redirectToLoginIfNotAuthenticated } from "@core/guards";
import { UserListComponent } from "./users/users-list/users-list.component";
import { PermissionGuard } from "@core/guards/permission.guard";



export default [
  {
    path: '',
    component: LayoutComponent,
    canActivate: [redirectToLoginIfNotAuthenticated()],
    children: [
      {
        path: '',
        component: UserListComponent,
        canActivate: [PermissionGuard('Users', 'Read')]
      },
    ]
  }
] as Routes;
