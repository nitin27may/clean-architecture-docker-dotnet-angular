import { Component, inject, OnInit, signal } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { PermissionService } from '@core/services/permission.service';
import { PageService } from '@core/services/page.service';
import { OperationService } from '@core/services/operation.service';
import { Permission } from '@core/models/permission.interface';
import { Page } from '@core/models/page.interface';
import { Operation } from '@core/models/operation.interface';

@Component({
  selector: 'app-permissions',
  templateUrl: './permissions.component.html',
  styleUrls: ['./permissions.component.scss']
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
  permissionForm: FormGroup;

  constructor() {
    this.permissionForm = this.fb.group({
      pageId: ['', Validators.required],
      operationId: ['', Validators.required],
      description: ['']
    });
  }

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
