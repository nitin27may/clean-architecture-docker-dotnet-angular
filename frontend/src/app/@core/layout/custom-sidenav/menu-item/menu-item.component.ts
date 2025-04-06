import { Component, OnInit, computed, input, signal, EventEmitter, Output } from '@angular/core';
import { RouterLinkActive, RouterModule, Router, NavigationEnd } from '@angular/router';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import { CommonModule } from '@angular/common';
import { trigger, transition, style, animate } from '@angular/animations';
import { MenuItem } from '@core/layout/custom-sidenav/menu-items';
import { filter } from 'rxjs/operators';

@Component({
  selector: 'app-menu-item',
  standalone: true,
  imports: [CommonModule, RouterModule, RouterLinkActive, MatListModule, MatIconModule],
  template: `
    <a
      mat-list-item
      [style.--mat-list-list-item-leading-icon-start-space]="indentation()"
      [routerLink]="makeRoutePath()"
      routerLinkActive
      #rla="routerLinkActive"
      [routerLinkActiveOptions]="{
        exact: item().exact || isRootRoute() || false
      }"
      [class.active-route]="isItemActive(rla.isActive)"
      [activated]="isItemActive(rla.isActive)"
      (click)="toggleNestedItems()"
      class="menu-item"
    >
      <mat-icon
        [fontSet]="isItemActive(rla.isActive) ? 'material-icons' : 'material-icons-outlined'"
        matListItemIcon
        [ngClass]="{'active-icon': isItemActive(rla.isActive)}"
        >{{ item().icon }}</mat-icon
      >
      <span matListItemTitle [ngClass]="{'active-text': isItemActive(rla.isActive), 'opacity-0': collapsed()}"
        >{{ item().label }}</span>
      @if(hasSubItems()) {
        <span matListItemMeta [class.hidden]="collapsed()">
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
            (activeStatusChanged)="childActiveStatusChanged($event)"
          />
        }
      </div>
    }
  `,
  styles: `
    :host {
      display: block;
    }

    .menu-item {
      margin: 4px 8px;
      border-radius: 6px;
      overflow: hidden;
      transition: all 300ms ease-in-out;
    }

    :host ::ng-deep {
      .active-route {
        background-color: var(--primary-lighter, rgba(0, 120, 212, 0.1)) !important;
      }

      .active-icon {
        color: var(--primary, #0078D4) !important;
      }

      .active-text {
        font-weight: 500 !important;
        color: var(--primary, #0078D4) !important;
      }

      .mat-mdc-list-item {
        --mdc-list-list-item-container-shape: 6px;
      }
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
export class MenuItemComponent implements OnInit {
  item = input.required<MenuItem>();
  collapsed = input.required<boolean>();
  routeHistory = input('');

  @Output() activeStatusChanged = new EventEmitter<boolean>();

  level = computed(() => this.routeHistory().split('/').filter(Boolean).length);
  indentation = computed(() =>
    this.collapsed() ? '16px' : `${16 + this.level() * 16}px`
  );

  nestedItemOpen = signal(false);
  private childIsActive = signal(false);

  constructor(private router: Router) {}

  ngOnInit() {
    // Listen for route changes to update active status
    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd)
    ).subscribe(() => {
      this.checkActiveStatus();
    });

    // Initial check
    this.checkActiveStatus();
  }

  private checkActiveStatus() {
    const currentPath = this.router.url;
    const itemPath = this.makeRoutePath();

    if (!itemPath) return;

    // Special handling for root route
    if (itemPath === '/') {
      const isExactMatch = currentPath === '/' || currentPath === '';
      this.childIsActive.set(isExactMatch);
      if (isExactMatch) {
        this.activeStatusChanged.emit(true);
      }
      return;
    }

    // For other routes, check if the current path starts with the item path
    // and make sure it's either an exact match or followed by a slash
    const isActive = currentPath === itemPath ||
                    (currentPath.startsWith(itemPath) &&
                     (currentPath.length === itemPath.length || currentPath[itemPath.length] === '/'));

    if (isActive) {
      this.nestedItemOpen.set(true);
      this.childIsActive.set(true);
      this.activeStatusChanged.emit(true);
    } else {
      this.childIsActive.set(false);
    }
  }

  isRootRoute(): boolean {
    return this.item().route === '/';
  }

  isItemActive(routerLinkActive: boolean): boolean {
    // For the root route, we need exact matching
    if (this.isRootRoute()) {
      return routerLinkActive && (this.router.url === '/' || this.router.url === '');
    }

    // For other routes, use RouterLinkActive or child active status
    return routerLinkActive || this.isChildActive();
  }

  hasSubItems(): boolean {
    return !!this.item().subItems && this.item().subItems.length > 0;
  }

  makeRoutePath() {
    if (!this.item().route) return undefined;
    return this.item().route.startsWith('/')
      ? this.item().route
      : `${this.routeHistory()}/${this.item().route}`;
  }

  makeBasePath() {
    if (!this.item().route) return this.routeHistory();
    return this.item().route.startsWith('/')
      ? this.item().route
      : `${this.routeHistory()}/${this.item().route}`;
  }

  toggleNestedItems() {
    if (this.hasSubItems()) {
      this.nestedItemOpen.update(value => !value);
    }
  }

  isChildActive(): boolean {
    return this.childIsActive();
  }

  childActiveStatusChanged(isActive: boolean) {
    this.childIsActive.set(isActive);
    if (isActive) {
      // Expand menu if a child is active
      this.nestedItemOpen.set(true);
      // Notify parent that this item is active
      this.activeStatusChanged.emit(true);
    }
  }
}
