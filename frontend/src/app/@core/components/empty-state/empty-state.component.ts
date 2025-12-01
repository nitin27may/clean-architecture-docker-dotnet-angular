import {
  Component,
  ChangeDetectionStrategy,
  input,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';

/**
 * Empty state component for displaying when no data is available.
 * 
 * Usage:
 * ```html
 * <app-empty-state
 *   icon="contacts"
 *   title="No contacts found"
 *   description="Create your first contact to get started"
 *   actionText="Add Contact"
 *   actionRoute="/contacts/create">
 * </app-empty-state>
 * ```
 */
@Component({
  selector: 'app-empty-state',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    MatIconModule,
    MatButtonModule,
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <div class="flex flex-col items-center justify-center py-16 px-4 text-center"
         role="status"
         [attr.aria-label]="title()">
      <div class="w-24 h-24 rounded-full bg-surface-container flex items-center justify-center mb-6">
        <mat-icon class="text-6xl text-on-surface-variant">{{ icon() }}</mat-icon>
      </div>
      
      <h3 class="text-xl font-semibold text-on-surface mb-2">{{ title() }}</h3>
      
      @if (description()) {
        <p class="text-on-surface-variant max-w-md mb-6">{{ description() }}</p>
      }
      
      @if (actionText() && actionRoute()) {
        <button 
          mat-raised-button 
          color="primary" 
          [routerLink]="actionRoute()"
          class="min-w-[140px]">
          @if (actionIcon()) {
            <mat-icon class="mr-2">{{ actionIcon() }}</mat-icon>
          }
          {{ actionText() }}
        </button>
      }
    </div>
  `,
  styles: [`
    :host {
      display: block;
    }
  `]
})
export class EmptyStateComponent {
  /** Material icon name to display */
  icon = input<string>('inbox');
  
  /** Main title for the empty state */
  title = input.required<string>();
  
  /** Optional description text */
  description = input<string>();
  
  /** Optional action button text */
  actionText = input<string>();
  
  /** Optional route for the action button */
  actionRoute = input<string>();
  
  /** Optional icon for the action button */
  actionIcon = input<string>('add');
}
