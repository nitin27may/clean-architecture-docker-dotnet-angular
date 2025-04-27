import { Component, inject, Input, OnInit, signal, DestroyRef, effect } from '@angular/core';
import { RouterModule, Router, NavigationEnd } from '@angular/router';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import { CommonModule } from '@angular/common';
import { filter } from 'rxjs/operators';
import { MenuService, MenuItem } from '@core/layout/custom-sidenav/menu-items';
import { PermissionService } from '@core/services/permission.service';
import { AuthStateService } from '@core/services/auth-state.service';
import { MenuItemComponent } from '@core/layout/custom-sidenav/menu-item/menu-item.component';

@Component({
  selector: 'app-custom-sidenav',
  templateUrl: './custom-sidenav.component.html',
  styleUrls: ['./custom-sidenav.component.scss'],
  standalone: true,
  imports: [CommonModule, RouterModule, MatListModule, MatIconModule, MenuItemComponent]
})
export class CustomSidenavComponent implements OnInit {
  @Input() set collapsed(value: boolean) {
    this.isCollapsed.set(value);
  }
  
  private menuService = inject(MenuService);
  private permissionService = inject(PermissionService);
  private authState = inject(AuthStateService);
  private destroyRef = inject(DestroyRef);
  private router = inject(Router);

  // Convert class properties to signals
  isCollapsed = signal<boolean>(false);
  menuItems = signal<MenuItem[]>([]);

  constructor() {
    // Create the effect in the constructor (injection context)
    effect(() => {
      // Access the signal to establish dependency
      const user = this.authState.getCurrentUser()();
      // Update menu items whenever user changes
      this.menuItems.set(this.menuService.getMenuItems());
    });
  }

  ngOnInit() {
    // Listen to route changes to update menu item active states
    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd)
    ).subscribe(() => {
      // This will trigger a check on the current URL
      this.checkActiveRoutes();
    });

    // Initial check
    this.checkActiveRoutes();
  }

  hasPermission(permission?: string): boolean {
    if (!permission) return true;
    const [page, operation] = permission.split('.');
    return this.permissionService.hasPermission(page, operation);
  }

  onMenuItemActiveStatusChanged(isActive: boolean) {
    // This method is called when a child menu item changes its active status
    // We might use this for analytics or other side effects
  }

  private checkActiveRoutes() {
    // Force detection of active routes when navigation occurs
    // This is a workaround for cases where RouterLinkActive doesn't detect changes
    setTimeout(() => {
      // This timeout forces Angular to run change detection
    }, 0);
  }
}
