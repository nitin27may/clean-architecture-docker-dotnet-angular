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
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
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
    MatIconModule,
    MatProgressSpinnerModule
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
  loading = signal<boolean>(false);
  loadingRoles = signal<boolean>(false);
  
  userRoleForm = this.fb.group({
    userId: ['', Validators.required],
    roleIds: [[] as string[], Validators.required]
  });

  ngOnInit(): void {
    this.loadUsers();
    this.loadRoles();
  }

  loadUsers(): void {
    this.loading.set(true);
    this.userService.getAll().subscribe({
      next: (users) => {
        console.log('Users loaded for dropdown:', users);
        this.users.set(users);
        this.loading.set(false);
      },
      error: (err) => {
        console.error('Failed to load users:', err);
        this.snackBar.open('Failed to load users', 'Close', { duration: 3000 });
        this.loading.set(false);
      }
    });
  }

  loadRoles(): void {
    this.loadingRoles.set(true);
    this.roleService.getRoles().subscribe({
      next: (roles) => {
        this.roles.set(roles);
        this.loadingRoles.set(false);
      },
      error: (err) => {
        console.error('Failed to load roles:', err);
        this.snackBar.open('Failed to load roles', 'Close', { duration: 3000 });
        this.loadingRoles.set(false);
      }
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
    this.loadingRoles.set(true);
    this.userRoleService.getUserRoles(userId).subscribe({
      next: (response) => {
        // Handle the response properly - ensure we have an array of roles
        const roleIds =  response?.roles.map(r => r.id);
        this.selectedRoles.set(roleIds);
        this.userRoleForm.patchValue({ roleIds });
        this.loadingRoles.set(false);
      },
      error: (err) => {
        console.error('Failed to load user roles:', err);
        this.snackBar.open('Failed to load user roles', 'Close', { duration: 3000 });
        this.loadingRoles.set(false);
      }
    });
  }

  saveUserRoles(): void {
    if (this.userRoleForm.invalid) {
      return;
    }

    this.loading.set(true);
    const userRoleData = this.userRoleForm.value;
    this.userRoleService.updateUserRoles(userRoleData.userId!, userRoleData.roleIds!).subscribe({
      next: () => {
        this.snackBar.open('Roles assigned successfully', 'Close', { duration: 3000 });
        this.loading.set(false);
        this.resetForm();
      },
      error: (err) => {
        console.error('Failed to assign roles:', err);
        this.snackBar.open('Failed to assign roles', 'Close', { duration: 3000 });
        this.loading.set(false);
      }
    });
  }

  resetForm(): void {
    this.selectedUser.set(null);
    this.selectedRoles.set([]);
    
    // Reset form with initial values to avoid validation errors
    this.userRoleForm.reset();
    
    // Mark the form as pristine and untouched to prevent validation errors
    this.userRoleForm.markAsPristine();
    this.userRoleForm.markAsUntouched();
    
    // // Update form controls to be pristine and untouched as well
    // Object.keys(this.userRoleForm.controls).forEach(key => {
    //   const control = this.userRoleForm.get(key);
    //   control?.markAsPristine();
    //   control?.markAsUntouched();
    // });
  }
  
  getRoleName(roleId: string): string {
    const role = this.roles().find(r => r.id === roleId);
    return role ? role.name : 'Unknown Role';
  }
}
