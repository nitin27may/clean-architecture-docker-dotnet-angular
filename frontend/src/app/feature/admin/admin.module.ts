import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatDialogModule } from '@angular/material/dialog';
import { MatTableModule } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';

import { PagesComponent } from './pages/pages.component';
import { OperationsComponent } from './operations/operations.component';
import { PermissionsComponent } from './permissions/permissions.component';
import { RolePermissionsComponent } from './role-permissions/role-permissions.component';
import { UserRolesComponent } from './user-roles/user-roles.component';
import { UsersComponent } from './users/users.component';

const routes: Routes = [
  { path: 'pages', component: PagesComponent },
  { path: 'operations', component: OperationsComponent },
  { path: 'permissions', component: PermissionsComponent },
  { path: 'role-permissions', component: RolePermissionsComponent },
  { path: 'user-roles', component: UserRolesComponent },
  { path: 'users', component: UsersComponent }
];

@NgModule({
  declarations: [
    PagesComponent,
    OperationsComponent,
    PermissionsComponent,
    RolePermissionsComponent,
    UserRolesComponent,
    UsersComponent
  ],
  imports: [
    CommonModule,
    RouterModule.forChild(routes),
    ReactiveFormsModule,
    MatButtonModule,
    MatInputModule,
    MatSelectModule,
    MatSnackBarModule,
    MatDialogModule,
    MatTableModule,
    MatIconModule
  ]
})
export class AdminModule { }
