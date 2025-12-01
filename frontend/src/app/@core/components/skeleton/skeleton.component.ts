import {
  Component,
  ChangeDetectionStrategy,
  input,
  computed,
} from '@angular/core';
import { CommonModule } from '@angular/common';

/**
 * Skeleton loader component for displaying loading states.
 * 
 * Usage:
 * ```html
 * <app-skeleton type="table" [rows]="5" [columns]="4" />
 * <app-skeleton type="card" />
 * <app-skeleton type="details" />
 * <app-skeleton type="form" [rows]="6" />
 * ```
 */
@Component({
  selector: 'app-skeleton',
  standalone: true,
  imports: [CommonModule],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <div class="animate-pulse" role="status" aria-label="Loading content">
      @switch (type()) {
        @case ('table') {
          <!-- Table header skeleton -->
          <div class="flex gap-4 p-4 bg-surface-container-high rounded-t-lg border-b border-outline-variant">
            @for (col of columnsArray(); track $index) {
              <div class="flex-1 h-4 bg-surface-container-highest rounded"></div>
            }
          </div>
          <!-- Table rows skeleton -->
          @for (row of rowsArray(); track $index) {
            <div class="flex gap-4 p-4 border-b border-outline-variant last:border-b-0">
              @for (col of columnsArray(); track $index) {
                <div 
                  class="flex-1 h-4 bg-surface-container rounded"
                  [style.width]="getRandomWidth()">
                </div>
              }
            </div>
          }
        }
        
        @case ('card') {
          <div class="p-6 bg-surface-container rounded-lg">
            <!-- Card header -->
            <div class="flex items-center gap-4 mb-6">
              <div class="w-12 h-12 bg-surface-container-highest rounded-full"></div>
              <div class="flex-1">
                <div class="h-5 bg-surface-container-highest rounded w-1/3 mb-2"></div>
                <div class="h-3 bg-surface-container-highest rounded w-1/4"></div>
              </div>
            </div>
            <!-- Card content -->
            <div class="space-y-3">
              <div class="h-4 bg-surface-container-highest rounded w-full"></div>
              <div class="h-4 bg-surface-container-highest rounded w-5/6"></div>
              <div class="h-4 bg-surface-container-highest rounded w-4/6"></div>
            </div>
          </div>
        }
        
        @case ('details') {
          <div class="p-6 bg-surface-container rounded-lg">
            <!-- Header with avatar -->
            <div class="flex items-center gap-4 mb-8">
              <div class="w-16 h-16 bg-surface-container-highest rounded-full"></div>
              <div class="flex-1">
                <div class="h-6 bg-surface-container-highest rounded w-1/3 mb-2"></div>
                <div class="h-4 bg-surface-container-highest rounded w-1/4"></div>
              </div>
            </div>
            <!-- Details grid -->
            <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
              @for (item of rowsArray(); track $index) {
                <div class="space-y-2">
                  <div class="h-3 bg-surface-container-highest rounded w-1/4"></div>
                  <div class="h-5 bg-surface-container-highest rounded w-3/4"></div>
                </div>
              }
            </div>
          </div>
        }
        
        @case ('form') {
          <div class="p-6 bg-surface-container rounded-lg">
            <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
              @for (field of rowsArray(); track $index) {
                <div class="space-y-2">
                  <div class="h-3 bg-surface-container-highest rounded w-1/4"></div>
                  <div class="h-12 bg-surface-container-highest rounded w-full"></div>
                </div>
              }
            </div>
            <!-- Action buttons -->
            <div class="flex justify-end gap-3 mt-8">
              <div class="h-10 bg-surface-container-highest rounded w-24"></div>
              <div class="h-10 bg-surface-container-highest rounded w-32"></div>
            </div>
          </div>
        }
        
        @case ('list') {
          @for (row of rowsArray(); track $index) {
            <div class="flex items-center gap-4 p-4 border-b border-outline-variant last:border-b-0">
              <div class="w-10 h-10 bg-surface-container-highest rounded-full"></div>
              <div class="flex-1">
                <div class="h-4 bg-surface-container-highest rounded w-1/3 mb-2"></div>
                <div class="h-3 bg-surface-container-highest rounded w-1/2"></div>
              </div>
              <div class="h-8 bg-surface-container-highest rounded w-20"></div>
            </div>
          }
        }
        
        @default {
          <!-- Generic block skeleton -->
          <div class="space-y-4">
            @for (row of rowsArray(); track $index) {
              <div class="h-4 bg-surface-container-highest rounded" [style.width]="getRandomWidth()"></div>
            }
          </div>
        }
      }
      <span class="sr-only">Loading...</span>
    </div>
  `,
  styles: [`
    :host {
      display: block;
    }
    
    .animate-pulse {
      animation: pulse 2s cubic-bezier(0.4, 0, 0.6, 1) infinite;
    }
    
    @keyframes pulse {
      0%, 100% {
        opacity: 1;
      }
      50% {
        opacity: 0.5;
      }
    }
  `]
})
export class SkeletonComponent {
  /** Type of skeleton to display */
  type = input<'table' | 'card' | 'details' | 'form' | 'list' | 'block'>('table');
  
  /** Number of rows to display (for table, list, form, block types) */
  rows = input<number>(5);
  
  /** Number of columns to display (for table type) */
  columns = input<number>(4);
  
  /** Create array from rows count for iteration */
  rowsArray = computed(() => Array(this.rows()).fill(0));
  
  /** Create array from columns count for iteration */
  columnsArray = computed(() => Array(this.columns()).fill(0));
  
  /** Get random width for variety in skeleton appearance */
  getRandomWidth(): string {
    const widths = ['60%', '70%', '80%', '90%', '100%'];
    return widths[Math.floor(Math.random() * widths.length)];
  }
}
