import { Component, inject, OnInit, signal } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { RoleService } from '@core/services/role.service';
import { PermissionService } from '@core/services/permission.service';
import { RolePermissionService } from '@core/services/role-permission.service';
import { Role } from '@core/models/role.interface';
import { Permission } from '@core/models/permission.interface';

@Component({
  selector: 'app-role-permissions',
  templateUrl: './role-permissions.component.html',
  styleUrls: ['./role-permissions.component.scss']
})
export class RolePermissionsComponent implements OnInit {
  private fb = inject(FormBuilder);
  private roleService = inject(RoleService);
  private permissionService = inject(PermissionService);
  private rolePermissionService = inject(RolePermissionService);
  private snackBar = inject(MatSnackBar);

  roles = signal<Role[]>([]);
  permissions = signal<Permission[]>([]);
  selectedRole = signal<Role | null>(null);
  selectedPermissions = signal<string[]>([]);
  rolePermissionForm: FormGroup;

  constructor() {
    this.rolePermissionForm = this.fb.group({
      roleId: ['', Validators.required],
      permissionIds: [[], Validators.required]
    });
  }

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

  selectRole(role: Role): void {
    this.selectedRole.set(role);
    this.rolePermissionForm.patchValue({ roleId: role.id });
    this.loadRolePermissions(role.id);
  }

  loadRolePermissions(roleId: string): void {
    this.rolePermissionService.getRolePermissions(roleId).subscribe({
      next: (permissions) => this.selectedPermissions.set(permissions.map(p => p.id)),
      error: (err) => this.snackBar.open('Failed to load role permissions', 'Close', { duration: 3000 })
    });
  }

  saveRolePermissions(): void {
    if (this.rolePermissionForm.invalid) {
      return;
    }

    const rolePermissionData = this.rolePermissionForm.value;
    this.rolePermissionService.assignPermissionsToRole(rolePermissionData.roleId, rolePermissionData.permissionIds).subscribe({
      next: () => {
        this.snackBar.open('Permissions assigned successfully', 'Close', { duration: 3000 });
        this.resetForm();
      },
      error: (err) => this.snackBar.open('Failed to assign permissions', 'Close', { duration: 3000 })
    });
  }

  resetForm(): void {
    this.selectedRole.set(null);
    this.selectedPermissions.set([]);
    this.rolePermissionForm.reset();
  }
}
