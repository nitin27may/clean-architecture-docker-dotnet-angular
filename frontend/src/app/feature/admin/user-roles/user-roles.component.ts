import { Component, inject, OnInit, signal } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { UserService } from '@core/services/user.service';
import { RoleService } from '@core/services/role.service';
import { User } from '@core/models/user.interface';
import { Role } from '@core/models/role.interface';

@Component({
  selector: 'app-user-roles',
  templateUrl: './user-roles.component.html',
  styleUrls: ['./user-roles.component.scss']
})
export class UserRolesComponent implements OnInit {
  private fb = inject(FormBuilder);
  private userService = inject(UserService);
  private roleService = inject(RoleService);
  private snackBar = inject(MatSnackBar);

  users = signal<User[]>([]);
  roles = signal<Role[]>([]);
  selectedUser = signal<User | null>(null);
  selectedRoles = signal<string[]>([]);
  userRoleForm: FormGroup;

  constructor() {
    this.userRoleForm = this.fb.group({
      userId: ['', Validators.required],
      roleIds: [[], Validators.required]
    });
  }

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

  selectUser(user: User): void {
    this.selectedUser.set(user);
    this.userRoleForm.patchValue({ userId: user.id });
    this.loadUserRoles(user.id);
  }

  loadUserRoles(userId: string): void {
    this.userService.getUserRoles(userId).subscribe({
      next: (roles) => this.selectedRoles.set(roles.map(r => r.id)),
      error: (err) => this.snackBar.open('Failed to load user roles', 'Close', { duration: 3000 })
    });
  }

  saveUserRoles(): void {
    if (this.userRoleForm.invalid) {
      return;
    }

    const userRoleData = this.userRoleForm.value;
    this.userService.updateUserRoles(userRoleData.userId, userRoleData.roleIds).subscribe({
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
}
