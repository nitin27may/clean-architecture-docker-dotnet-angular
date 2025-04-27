import { Component, inject, OnInit, signal } from '@angular/core';
import { FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { RoleService } from './role.service';
import { Role } from '@core/models/role.interface';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatTableModule } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-roles',
  templateUrl: './roles.component.html',
  styleUrls: ['./roles.component.scss'],
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatTableModule,
    MatIconModule
  ]
})
export class RolesComponent implements OnInit {
  private fb = inject(FormBuilder);
  private roleService = inject(RoleService);
  private snackBar = inject(MatSnackBar);
  private dialog = inject(MatDialog);

  roles = signal<Role[]>([]);
  selectedRole = signal<Role | null>(null);
  roleForm = this.fb.group({
    name: ['', Validators.required],
    description: ['']
  });
  
  displayedColumns: string[] = ['name', 'description', 'actions'];

  ngOnInit(): void {
    this.loadRoles();
  }

  loadRoles(): void {
    this.roleService.getRoles().subscribe({
      next: (roles) => this.roles.set(roles),
      error: (err) => this.snackBar.open('Failed to load roles', 'Close', { duration: 3000 })
    });
  }

  selectRole(role: Role): void {
    this.selectedRole.set(role);
    this.roleForm.patchValue(role);
  }

  saveRole(): void {
    if (this.roleForm.invalid) {
      return;
    }

    const roleData = this.roleForm.value as Role;
    if (this.selectedRole()) {
      this.roleService.updateRole(this.selectedRole()!.id, roleData).subscribe({
        next: () => {
          this.snackBar.open('Role updated successfully', 'Close', { duration: 3000 });
          this.loadRoles();
          this.resetForm();
        },
        error: (err) => this.snackBar.open('Failed to update role', 'Close', { duration: 3000 })
      });
    } else {
      this.roleService.createRole(roleData).subscribe({
        next: () => {
          this.snackBar.open('Role created successfully', 'Close', { duration: 3000 });
          this.loadRoles();
          this.resetForm();
        },
        error: (err) => this.snackBar.open('Failed to create role', 'Close', { duration: 3000 })
      });
    }
  }

  deleteRole(role: Role): void {
    this.roleService.deleteRole(role.id).subscribe({
      next: () => {
        this.snackBar.open('Role deleted successfully', 'Close', { duration: 3000 });
        this.loadRoles();
      },
      error: (err) => this.snackBar.open('Failed to delete role', 'Close', { duration: 3000 })
    });
  }

  resetForm(): void {
    this.selectedRole.set(null);
    this.roleForm.reset();
  }
}