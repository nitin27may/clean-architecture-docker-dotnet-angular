import { Component, computed, input, signal } from '@angular/core';
import { RouterLinkActive, RouterModule } from '@angular/router';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import { CommonModule } from '@angular/common';
import { trigger, transition, style, animate } from '@angular/animations';
import { MenuItem } from '../menu-items';

@Component({
  selector: 'app-menu-item',
  standalone: true,
  imports: [CommonModule, RouterModule, RouterLinkActive, MatListModule, MatIconModule],
  template: `
    <a
      mat-list-item
      [style.--mat-list-list-item-leading-icon-start-space]="indentation()"
      [routerLink]="makeRoutePath()"
      (click)="toggleNestedItems()"
      routerLinkActive
      #rla="routerLinkActive"
      [routerLinkActiveOptions]="{ exact: item().exact }"
      [activated]="rla.isActive"
    >
      <mat-icon
        [fontSet]="rla.isActive ? 'material-icons' : 'material-icons-outlined'"
        matListItemIcon
        >{{ item().icon }}</mat-icon
      >
      @if(!collapsed()) {
        <span matListItemTitle>{{ item().label }}</span>
      }
      @if(hasSubItems()) {
        <span matListItemMeta>
          <mat-icon>{{ nestedItemOpen() ? 'expand_less' : 'expand_more' }}</mat-icon>
        </span>
      }
    </a>
    @if (nestedItemOpen() && hasSubItems()) {
      <div @expandContractMenu>
        @for(subItem of item().subItems || []; track subItem.route) {
          <app-menu-item
            [item]="subItem"
            [routeHistory]="makeBasePath()"
            [collapsed]="collapsed()"
          />
        }
      </div>
    }
  `,
  styles: `
    :host * {
      transition-property: margin-inline-start, opacity, height;
      transition-duration: 500ms;
      transition-timing-function: ease-in-out;
    }
  `,
  animations: [
    trigger('expandContractMenu', [
      transition(':enter', [
        style({ opacity: 0, height: '0px' }),
        animate('500ms ease-in-out', style({ opacity: 1, height: '*' })),
      ]),
      transition(':leave', [
        animate('500ms ease-in-out', style({ opacity: 0, height: '0px' })),
      ]),
    ]),
  ],
})
export class MenuItemComponent {
  item = input.required<MenuItem>();
  collapsed = input.required<boolean>();
  routeHistory = input('');

  level = computed(() => this.routeHistory().split('/').length - 1);
  indentation = computed(() =>
    this.collapsed() ? '16px' : `${16 + this.level() * 16}px`
  );

  nestedItemOpen = signal(false);

  // Helper method to safely check for subItems
  hasSubItems(): boolean {
    return !!this.item().subItems && this.item().subItems.length > 0;
  }

  makeRoutePath() {
    return this.item().route?.startsWith('/')
      ? this.item().route
      : `${this.routeHistory()}/${this.item().route}`;
  }

  makeBasePath() {
    return this.item().route?.startsWith('/')
      ? ''
      : `${this.routeHistory()}/${this.item().route}`;
  }

  toggleNestedItems() {
    if (this.hasSubItems()) {
      this.nestedItemOpen.update(value => !value);
    }
  }
}
