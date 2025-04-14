import { Component, inject, OnInit, signal } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { UserService } from '@core/services/user.service';
import { RoleService } from '../roles/role.service';
import { UserRoleService } from './user-role.service';
import { User } from '@core/models/user.interface';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatTableModule } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { CommonModule } from '@angular/common';
import { Role } from "@core/models/role.interface";
import { computed } from '@angular/core';

@Component({
  selector: 'app-user-roles',
  templateUrl: './user-roles.component.html',
  styleUrls: ['./user-roles.component.scss'],
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
export class UserRolesComponent implements OnInit {
  private fb = inject(FormBuilder);
  private userService = inject(UserService);
  private roleService = inject(RoleService);
  private userRoleService = inject(UserRoleService);
  private snackBar = inject(MatSnackBar);

  users = signal<User[]>([]);
  roles = signal<Role[]>([]);
  selectedUser = signal<User | null>(null);
  selectedRoles = signal<string[]>([]);
  
  userRoleForm = this.fb.group({
    userId: ['', Validators.required],
    roleIds: [[] as string[], Validators.required]
  });

  ngOnInit(): void {
    this.loadUsers();
    this.loadRoles();
  }

  loadUsers(): void {
    this.userService.getAll().subscribe({
      next: (users) => this.users.set(users),
      error: (err) => this.snackBar.open('Failed to load users', 'Close', { duration: 3000 })
    });
  }

  loadRoles(): void {
    this.roleService.getRoles().subscribe({
      next: (roles) => this.roles.set(roles),
      error: (err) => this.snackBar.open('Failed to load roles', 'Close', { duration: 3000 })
    });
  }

  onUserChange(userId: string): void {
    if (!userId) {
      this.selectedUser.set(null);
      this.selectedRoles.set([]);
      return;
    }
    
    // Find and set the selected user
    const user = this.users().find(u => u.id === userId);
    if (user) {
      this.selectedUser.set(user);
      this.loadUserRoles(userId);
    }
  }
  
  loadUserRoles(userId: string): void {
    this.userRoleService.getUserRoles(userId).subscribe({
      next: (roles) => {
        const roleIds = roles.map(r => r.id);
        this.selectedRoles.set(roleIds);
        this.userRoleForm.patchValue({ roleIds });
      },
      error: (err) => this.snackBar.open('Failed to load user roles', 'Close', { duration: 3000 })
    });
  }

  saveUserRoles(): void {
    if (this.userRoleForm.invalid) {
      return;
    }

    const userRoleData = this.userRoleForm.value;
    this.userRoleService.updateUserRoles(userRoleData.userId!, userRoleData.roleIds!).subscribe({
      next: () => {
        this.snackBar.open('Roles assigned successfully', 'Close', { duration: 3000 });
        this.resetForm();
      },
      error: (err) => this.snackBar.open('Failed to assign roles', 'Close', { duration: 3000 })
    });
  }

  resetForm(): void {
    this.selectedUser.set(null);
    this.selectedRoles.set([]);
    this.userRoleForm.reset();
  }
  
  getRoleName(roleId: string): string {
    const role = this.roles().find(r => r.id === roleId);
    return role ? role.name : 'Unknown Role';
  }
}
