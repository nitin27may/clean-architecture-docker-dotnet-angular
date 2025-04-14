import { Component, OnInit, inject, signal } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { RoleService } from '../roles/role.service';
import { PermissionService } from '../permissions/permission.service';
import { RolePermissionService } from './role-permission.service';
import { Permission } from '@core/models/permission.interface';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatTableModule } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { CommonModule } from '@angular/common';
import { Role } from "@core/models/role.interface";

@Component({
  selector: 'app-role-permissions',
  templateUrl: './role-permissions.component.html',
  styleUrls: ['./role-permissions.component.scss'],
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatSelectModule,
    MatInputModule,
    MatButtonModule,
    MatTableModule,
    MatIconModule
  ]
})
export class RolePermissionsComponent implements OnInit {
  private fb = inject(FormBuilder);
  private roleService = inject(RoleService);
  private permissionService = inject(PermissionService);
  private rolePermissionService = inject(RolePermissionService);
  private snackBar = inject(MatSnackBar);

  roles = signal<Role[]>([]);
  permissions = signal<Permission[]>([]);
  selectedPermissions = signal<Permission[]>([]);
  
  rolePermissionForm = this.fb.group({
    roleId: ['', Validators.required],
    permissionIds: [[] as string[], Validators.required]
  });
  
  displayedColumns: string[] = ['pageName', 'operationName'];

  ngOnInit(): void {
    this.loadRoles();
    this.loadPermissions();
  }

  loadRoles(): void {
    this.roleService.getRoles().subscribe({
      next: (roles) => this.roles.set(roles),
      error: (err) => this.snackBar.open('Failed to load roles', 'Close', { duration: 3000 })
    });
  }

  loadPermissions(): void {
    this.permissionService.getPermissions().subscribe({
      next: (permissions) => this.permissions.set(permissions),
      error: (err) => this.snackBar.open('Failed to load permissions', 'Close', { duration: 3000 })
    });
  }

  onRoleChange(roleId: string): void {
    if (!roleId) {
      this.selectedPermissions.set([]);
      return;
    }
    
    this.rolePermissionService.getRolePermissions(roleId).subscribe({
      next: (permissions) => {
        this.selectedPermissions.set(permissions);
        this.rolePermissionForm.patchValue({
          permissionIds: permissions.map(p => p.id)
        });
      },
      error: (err) => this.snackBar.open('Failed to load role permissions', 'Close', { duration: 3000 })
    });
  }

  saveRolePermissions(): void {
    if (this.rolePermissionForm.invalid) {
      return;
    }

    const rolePermissionData = this.rolePermissionForm.value as { roleId: string; permissionIds: string[] };
    
    this.rolePermissionService.assignPermissionsToRole(rolePermissionData).subscribe({
      next: () => {
        this.snackBar.open('Role permissions updated successfully', 'Close', { duration: 3000 });
        this.onRoleChange(rolePermissionData.roleId);
      },
      error: (err) => this.snackBar.open('Failed to update role permissions', 'Close', { duration: 3000 })
    });
  }
}
