import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatTableModule } from '@angular/material/table';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatDividerModule } from '@angular/material/divider';
import { MatListModule } from '@angular/material/list';

import { RolePermissionMappingService } from '@core/services/role-permission-mapping.service';
import { NotificationService } from '@core/services/notification.service';
import { 
  RoleModel, 
  RolePermissionMappingResponse, 
  PageOperationResponse,
  RolePermissionMappingRequest,
  PageOperationRequest 
} from '@core/models/role-permission-mapping.model';

@Component({
  selector: 'app-role-permission-mapping',
  templateUrl: './role-permission-mapping.component.html',
  styleUrls: ['./role-permission-mapping.component.scss'],
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatCheckboxModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatTableModule,
    MatExpansionModule,
    MatDividerModule,
    MatListModule
  ]
})
export class RolePermissionMappingComponent implements OnInit {
  private rolePermissionService = inject(RolePermissionMappingService);
  private notificationService = inject(NotificationService);
  
  // State signals
  roles = signal<RoleModel[]>([]);
  selectedRoleId = signal<string>('');
  mapping = signal<RolePermissionMappingResponse | null>(null);
  loading = signal<boolean>(false);
  saving = signal<boolean>(false);
  
  // Track expanded panels
  expandedPanels = signal<{ [key: string]: boolean }>({});
  
  ngOnInit(): void {
    this.loadRoles();
  }
  
  /**
   * Load available roles for selection
   */
  loadRoles(): void {
    this.loading.set(true);
    this.rolePermissionService.getRoles().subscribe({
      next: (roles) => {
        this.roles.set(roles);
        this.loading.set(false);
        
        // Auto-select first role if available
        if (roles.length > 0) {
         // this.selectedRoleId.set(roles[0].id);
        //  this.loadRolePermissions(roles[0].id);
        }
      },
      error: (error) => {
        console.error('Error loading roles:', error);
        this.notificationService.error('Failed to load roles.');
        this.loading.set(false);
      }
    });
  }
  
  /**
   * Load permissions for the selected role
   */
  loadRolePermissions(roleId: string): void {
    this.loading.set(true);
    this.mapping.set(null);
    
    this.rolePermissionService.getRolePermissionMapping(roleId).subscribe({
      next: (response) => {
        this.mapping.set(response);
        this.loading.set(false);
        
        // Initialize expanded panels (all collapsed by default)
        const panels: { [key: string]: boolean } = {};
        response.pages.forEach(page => {
          panels[page.pageId] = false;
        });
        this.expandedPanels.set(panels);
      },
      error: (error) => {
        console.error('Error loading role permissions:', error);
        this.notificationService.error('Failed to load role permissions.');
        this.loading.set(false);
      }
    });
  }
  
  /**
   * Toggle selection of a specific operation
   */
  toggleOperation(page: PageOperationResponse, operationId: string): void {
    const currentMapping = this.mapping();
    if (!currentMapping) return;
    
    // Find the page and operation to toggle
    const updatedPages = currentMapping.pages.map(p => {
      if (p.pageId === page.pageId) {
        const updatedOperations = p.operations.map(op => {
          if (op.operationId === operationId) {
            return { ...op, isSelected: !op.isSelected };
          }
          return op;
        });
        return { ...p, operations: updatedOperations };
      }
      return p;
    });
    
    this.mapping.set({ ...currentMapping, pages: updatedPages });
  }
  
  /**
   * Toggle all operations for a page
   */
  toggleAllOperations(page: PageOperationResponse, selected: boolean): void {
    const currentMapping = this.mapping();
    if (!currentMapping) return;
    
    // Find the page and update all its operations
    const updatedPages = currentMapping.pages.map(p => {
      if (p.pageId === page.pageId) {
        const updatedOperations = p.operations.map(op => {
          return { ...op, isSelected: selected };
        });
        return { ...p, operations: updatedOperations };
      }
      return p;
    });
    
    this.mapping.set({ ...currentMapping, pages: updatedPages });
  }
  
  /**
   * Check if all operations in a page are selected
   */
  areAllOperationsSelected(page: PageOperationResponse): boolean {
    return page.operations.every(op => op.isSelected);
  }
  
  /**
   * Check if some operations in a page are selected
   */
  areSomeOperationsSelected(page: PageOperationResponse): boolean {
    return page.operations.some(op => op.isSelected) && !this.areAllOperationsSelected(page);
  }
  
  /**
   * Save the current role-permission mapping
   */
  saveMapping(): void {
    const currentMapping = this.mapping();
    if (!currentMapping) return;
    
    this.saving.set(true);
    
    // Prepare request data
    const request: RolePermissionMappingRequest = {
      roleId: currentMapping.roleId,
      permissions: []
    };
    
    // For each page, collect selected operation IDs
    currentMapping.pages.forEach(page => {
      const selectedOperationIds = page.operations
        .filter(op => op.isSelected)
        .map(op => op.operationId);
      
      if (selectedOperationIds.length > 0) {
        request.permissions.push({
          pageId: page.pageId,
          operationIds: selectedOperationIds
        });
      }
    });
    
    // Send request to API
    this.rolePermissionService.saveRolePermissionMapping(request).subscribe({
      next: () => {
        this.notificationService.success('Role permissions saved successfully.');
        this.saving.set(false);
      },
      error: (error) => {
        console.error('Error saving role permissions:', error);
        this.notificationService.error('Failed to save role permissions.');
        this.saving.set(false);
      }
    });
  }
  
  /**
   * Handle role selection change
   */
  onRoleChange(event: Event): void {
    const select = event.target as HTMLSelectElement;
    const roleId = select.value;
    if (roleId) {
      this.selectedRoleId.set(roleId);
      this.loadRolePermissions(roleId);
    }
  }
  
  /**
   * Toggle expansion state of a panel
   */
  togglePanel(pageId: string): void {
    const panels = { ...this.expandedPanels() };
    panels[pageId] = !panels[pageId];
    this.expandedPanels.set(panels);
  }
  
  /**
   * Check if a panel is expanded
   */
  isPanelExpanded(pageId: string): boolean {
    return this.expandedPanels()[pageId] || false;
  }
}