import { Injectable, inject } from '@angular/core';
import { PermissionService } from '../../services/permission.service';
import { AuthStateService } from '../../services/auth-state.service';

export interface MenuItem {
  icon: string;
  label: string;
  route?: string;
  exact?: boolean;
  permission?: string;
  subItems?: MenuItem[];
}

const defaultMenuItems: MenuItem[] = [
  {
    icon: 'dashboard',
    label: 'Dashboard',
    route: '/',
    exact: true
  }
];

@Injectable({
  providedIn: 'root'
})
export class MenuService {
  private permissionService = inject(PermissionService);
  private authState = inject(AuthStateService);

  getMenuItems(): MenuItem[] {
    const menuItems = [...defaultMenuItems];
    const user = this.authState.getCurrentUser()();

    if (!user?.rolePermissions) {
      return menuItems;
    }

    // Get unique pages from permissions
    const uniquePages = [...new Set(user.rolePermissions.map(p => p.pageName))];

    // Create menu items based on permissions
    uniquePages.forEach(pageName => {
      if (pageName.toLowerCase() === 'dashboard') return;

      const operations = this.permissionService.getPagePermissions(pageName);
      if (operations.includes('Read')) {
        menuItems.push({
          icon: this.getIconForPage(pageName),
          label: this.formatLabel(pageName),
          route: `/${pageName.toLowerCase()}`,
          exact: false,
          permission: `${pageName}.Read`
        });
      }
    });

    return this.sortMenuItems(menuItems);
  }

  private getIconForPage(pageName: string): string {
    const iconMap: Record<string, string> = {
      'Users': 'person',
      'Role': 'admin_panel_settings',
      'Contacts': 'contacts',
      'Permission': 'security',
      'Setting': 'settings'
    };

    return iconMap[pageName] || 'article';
  }

  private formatLabel(pageName: string): string {
    return pageName.replace(/([A-Z])/g, ' $1').trim();
  }

  private sortMenuItems(items: MenuItem[]): MenuItem[] {
    const order = ['Dashboard', 'User', 'Role', 'Permission'];
    return items.sort((a, b) => {
      const indexA = order.indexOf(a.label);
      const indexB = order.indexOf(b.label);
      if (indexA === -1 && indexB === -1) return a.label.localeCompare(b.label);
      if (indexA === -1) return 1;
      if (indexB === -1) return -1;
      return indexA - indexB;
    });
  }
}
