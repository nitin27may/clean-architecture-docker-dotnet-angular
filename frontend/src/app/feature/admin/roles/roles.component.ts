import { Component, ChangeDetectionStrategy, inject, OnInit, signal } from '@angular/core';
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
import { MatCardModule } from '@angular/material/card';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { CommonModule } from '@angular/common';
import { PageHeaderComponent } from '@core/components/page-header';
import { EmptyStateComponent } from '@core/components/empty-state';
import { SkeletonComponent } from '@core/components/skeleton';
import { ConfirmDialogComponent, ConfirmDialogData } from '@core/components/confirm-dialog';

@Component({
  selector: 'app-roles',
  templateUrl: './roles.component.html',
  styleUrls: ['./roles.component.scss'],
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatTableModule,
    MatIconModule,
    MatCardModule,
    MatTooltipModule,
    MatProgressSpinnerModule,
    PageHeaderComponent,
    EmptyStateComponent,
    SkeletonComponent
  ]
})
export class RolesComponent implements OnInit {
  private fb = inject(FormBuilder);
  private roleService = inject(RoleService);
  private snackBar = inject(MatSnackBar);
  private dialog = inject(MatDialog);

  roles = signal<Role[]>([]);
  selectedRole = signal<Role | null>(null);
  loading = signal<boolean>(false);
  roleForm = this.fb.group({
    name: ['', Validators.required],
    description: ['']
  });
  
  displayedColumns: string[] = ['name', 'description', 'actions'];

  ngOnInit(): void {
    this.loadRoles();
  }

  loadRoles(): void {
    this.loading.set(true);
    this.roleService.getRoles().subscribe({
      next: (roles) => {
        this.roles.set(roles);
        this.loading.set(false);
      },
      error: (err) => {
        this.snackBar.open('Failed to load roles', 'Close', { duration: 3000 });
        this.loading.set(false);
      }
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
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: {
        title: 'Delete Role',
        message: `Are you sure you want to delete the role "${role.name}"?`,
        confirmText: 'Delete',
        confirmColor: 'warn',
        icon: 'delete'
      } as ConfirmDialogData
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.loading.set(true);
        this.roleService.deleteRole(role.id).subscribe({
          next: () => {
            this.snackBar.open('Role deleted successfully', 'Close', { duration: 3000 });
            this.loadRoles();
          },
          error: (err) => {
            this.snackBar.open('Failed to delete role', 'Close', { duration: 3000 });
            this.loading.set(false);
          }
        });
      }
    });
  }

  resetForm(): void {
    this.selectedRole.set(null);
    this.roleForm.reset();
  }
}