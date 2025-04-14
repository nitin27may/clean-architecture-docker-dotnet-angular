import { Component, inject, OnInit, signal } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { PermissionService } from '../permissions/permission.service';
import { PageService } from '../pages/page.service';
import { OperationService } from '../operations/operation.service';
import { Permission } from '@core/models/permission.interface';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatTableModule } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { CommonModule } from '@angular/common';
import { Operation } from "@core/models/operation.interface";
import { Page } from "@core/models/page.interface";

@Component({
  selector: 'app-permissions',
  templateUrl: './permissions.component.html',
  styleUrls: ['./permissions.component.scss'],
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
export class PermissionsComponent implements OnInit {
  private fb = inject(FormBuilder);
  private permissionService = inject(PermissionService);
  private pageService = inject(PageService);
  private operationService = inject(OperationService);
  private snackBar = inject(MatSnackBar);

  permissions = signal<Permission[]>([]);
  pages = signal<Page[]>([]);
  operations = signal<Operation[]>([]);
  selectedPermission = signal<Permission | null>(null);
  permissionForm = this.fb.group({
    pageId: ['', Validators.required],
    operationId: ['', Validators.required],
    description: ['']
  });
  
  displayedColumns: string[] = ['pageName', 'operationName', 'description', 'actions'];

  ngOnInit(): void {
    this.loadPermissions();
    this.loadPages();
    this.loadOperations();
  }

  loadPermissions(): void {
    this.permissionService.getPermissions().subscribe({
      next: (permissions) => this.permissions.set(permissions),
      error: (err) => this.snackBar.open('Failed to load permissions', 'Close', { duration: 3000 })
    });
  }

  loadPages(): void {
    this.pageService.getPages().subscribe({
      next: (pages) => this.pages.set(pages),
      error: (err) => this.snackBar.open('Failed to load pages', 'Close', { duration: 3000 })
    });
  }

  loadOperations(): void {
    this.operationService.getOperations().subscribe({
      next: (operations) => this.operations.set(operations),
      error: (err) => this.snackBar.open('Failed to load operations', 'Close', { duration: 3000 })
    });
  }

  selectPermission(permission: Permission): void {
    this.selectedPermission.set(permission);
    this.permissionForm.patchValue(permission);
  }

  savePermission(): void {
    if (this.permissionForm.invalid) {
      return;
    }

    const permissionData = this.permissionForm.value as Permission;
    if (this.selectedPermission()) {
      this.permissionService.updatePermission(this.selectedPermission()!.id, permissionData).subscribe({
        next: () => {
          this.snackBar.open('Permission updated successfully', 'Close', { duration: 3000 });
          this.loadPermissions();
          this.resetForm();
        },
        error: (err) => this.snackBar.open('Failed to update permission', 'Close', { duration: 3000 })
      });
    } else {
      this.permissionService.createPermission(permissionData).subscribe({
        next: () => {
          this.snackBar.open('Permission created successfully', 'Close', { duration: 3000 });
          this.loadPermissions();
          this.resetForm();
        },
        error: (err) => this.snackBar.open('Failed to create permission', 'Close', { duration: 3000 })
      });
    }
  }

  deletePermission(permission: Permission): void {
    this.permissionService.deletePermission(permission.id).subscribe({
      next: () => {
        this.snackBar.open('Permission deleted successfully', 'Close', { duration: 3000 });
        this.loadPermissions();
      },
      error: (err) => this.snackBar.open('Failed to delete permission', 'Close', { duration: 3000 })
    });
  }

  resetForm(): void {
    this.selectedPermission.set(null);
    this.permissionForm.reset();
  }
}
