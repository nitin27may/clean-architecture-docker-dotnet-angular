import {
  Component,
  ChangeDetectionStrategy,
  inject,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatDialogModule, MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

/**
 * Data interface for the confirm dialog
 */
export interface ConfirmDialogData {
  /** Dialog title */
  title: string;
  /** Dialog message */
  message: string;
  /** Confirm button text (default: 'Confirm') */
  confirmText?: string;
  /** Cancel button text (default: 'Cancel') */
  cancelText?: string;
  /** Confirm button color (default: 'primary') */
  confirmColor?: 'primary' | 'accent' | 'warn';
  /** Optional icon for the dialog */
  icon?: string;
  /** Icon color class */
  iconColor?: string;
}

/**
 * Confirmation dialog component to replace browser confirm().
 * 
 * Usage:
 * ```typescript
 * const dialogRef = this.dialog.open(ConfirmDialogComponent, {
 *   data: {
 *     title: 'Delete Contact',
 *     message: 'Are you sure you want to delete this contact?',
 *     confirmText: 'Delete',
 *     confirmColor: 'warn',
 *     icon: 'delete'
 *   }
 * });
 * 
 * dialogRef.afterClosed().subscribe(result => {
 *   if (result) {
 *     // User confirmed
 *   }
 * });
 * ```
 */
@Component({
  selector: 'app-confirm-dialog',
  standalone: true,
  imports: [
    CommonModule,
    MatDialogModule,
    MatButtonModule,
    MatIconModule,
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <div class="p-6 max-w-md">
      <!-- Header with optional icon -->
      <div class="flex items-start gap-4 mb-4">
        @if (data.icon) {
          <div 
            class="shrink-0 w-12 h-12 rounded-full flex items-center justify-center"
            [class]="getIconBackgroundClass()">
            <mat-icon [class]="getIconColorClass()">{{ data.icon }}</mat-icon>
          </div>
        }
        <div class="flex-1">
          <h2 mat-dialog-title class="text-xl font-semibold m-0 mb-2 text-on-surface">
            {{ data.title }}
          </h2>
          <p mat-dialog-content class="text-on-surface-variant m-0">
            {{ data.message }}
          </p>
        </div>
      </div>
      
      <!-- Actions -->
      <mat-dialog-actions class="flex justify-end gap-3 mt-6 p-0">
        <button 
          mat-button 
          mat-dialog-close
          class="min-w-[80px]">
          {{ data.cancelText || 'Cancel' }}
        </button>
        <button 
          mat-raised-button 
          [color]="data.confirmColor || 'primary'"
          [mat-dialog-close]="true"
          class="min-w-[80px]"
          cdkFocusInitial>
          {{ data.confirmText || 'Confirm' }}
        </button>
      </mat-dialog-actions>
    </div>
  `,
  styles: [`
    :host {
      display: block;
    }
    
    ::ng-deep .mat-mdc-dialog-surface {
      border-radius: 16px !important;
    }
  `]
})
export class ConfirmDialogComponent {
  data = inject<ConfirmDialogData>(MAT_DIALOG_DATA);
  dialogRef = inject(MatDialogRef<ConfirmDialogComponent>);
  
  getIconBackgroundClass(): string {
    switch (this.data.confirmColor) {
      case 'warn':
        return 'bg-error-container';
      case 'accent':
        return 'bg-secondary-container';
      default:
        return 'bg-primary-container';
    }
  }
  
  getIconColorClass(): string {
    switch (this.data.confirmColor) {
      case 'warn':
        return 'text-error';
      case 'accent':
        return 'text-secondary';
      default:
        return 'text-primary';
    }
  }
}
