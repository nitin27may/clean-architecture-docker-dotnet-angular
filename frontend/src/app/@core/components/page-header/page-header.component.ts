import {
  Component,
  ChangeDetectionStrategy,
  input,
  output,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatTooltipModule } from '@angular/material/tooltip';

/**
 * Standardized page header component for consistent UI across all pages.
 * 
 * Usage:
 * ```html
 * <app-page-header
 *   title="Contacts"
 *   subtitle="Manage your contacts"
 *   icon="contacts"
 *   backRoute="/dashboard">
 *   <ng-container actions>
 *     <button mat-raised-button color="accent">Add New</button>
 *   </ng-container>
 * </app-page-header>
 * ```
 */
@Component({
  selector: 'app-page-header',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    MatIconModule,
    MatButtonModule,
    MatTooltipModule,
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <div class="mb-6 bg-primary text-white rounded-lg shadow-md overflow-hidden transition-all duration-300">
      <div class="p-6">
        <div class="flex flex-col md:flex-row md:items-center md:justify-between gap-4">
          <!-- Left side: Back button, Icon, Title, Subtitle -->
          <div class="flex items-center gap-4">
            @if (backRoute()) {
              <button 
                mat-icon-button 
                [routerLink]="backRoute()"
                class="text-white hover:bg-white/10 transition-colors"
                [attr.aria-label]="'Go back to ' + (backRouteLabel() || 'previous page')"
                matTooltip="Go back">
                <mat-icon>arrow_back</mat-icon>
              </button>
            }
            
            @if (icon()) {
              <div class="hidden sm:flex items-center justify-center w-12 h-12 rounded-full bg-white/10">
                <mat-icon class="text-3xl">{{ icon() }}</mat-icon>
              </div>
            }
            
            <div>
              <h1 class="text-2xl font-bold m-0 leading-tight">{{ title() }}</h1>
              @if (subtitle()) {
                <p class="text-sm opacity-90 mt-1 m-0">{{ subtitle() }}</p>
              }
            </div>
          </div>
          
          <!-- Right side: Action buttons slot -->
          <div class="flex flex-wrap items-center gap-2">
            <ng-content select="[actions]" />
          </div>
        </div>
      </div>
    </div>
  `,
  styles: [`
    :host {
      display: block;
    }
  `]
})
export class PageHeaderComponent {
  /** Main title displayed in the header */
  title = input.required<string>();
  
  /** Optional subtitle displayed below the title */
  subtitle = input<string>();
  
  /** Optional Material icon name displayed before the title */
  icon = input<string>();
  
  /** Optional route for back navigation button */
  backRoute = input<string>();
  
  /** Optional label for accessibility (aria-label for back button) */
  backRouteLabel = input<string>();
}
