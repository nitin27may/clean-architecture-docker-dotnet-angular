import { Injectable, inject, signal, computed } from '@angular/core';
import { PermissionService } from '@core/services/permission.service';
import { AuthStateService } from '@core/services/auth-state.service';

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
  
  // Using signals for reactive state management
  private menuItemsSignal = signal<MenuItem[]>([...defaultMenuItems]);
  menuItems = computed(() => this.menuItemsSignal());

  getMenuItems(): MenuItem[] {
    const menuItems = [...defaultMenuItems];
    const user = this.authState.getCurrentUser()();

    if (!user?.rolePermissions) {
      return menuItems;
    }

    // Map to store unique page information grouped by parent path
    const pageMap = new Map<string, { name: string, url: string, operations: string[] }>();
    
    // Group pages by parent path
    const groupedPages = new Map<string, Map<string, { name: string, url: string, operations: string[] }>>();

    // Process all role permissions to get unique pages with their URLs and operations
    user.rolePermissions.forEach(p => {
      const pageKey = p.pageName;
      const parentPath = this.getParentPath(p.pageUrl);
      
      if (!pageMap.has(pageKey)) {
        pageMap.set(pageKey, {
          name: p.pageName,
          url: p.pageUrl,
          operations: [p.operationName]
        });
      } else {
        if (!pageMap.get(pageKey)?.operations.includes(p.operationName)) {
          pageMap.get(pageKey)?.operations.push(p.operationName);
        }
      }
      
      // Group by parent path
      if (!groupedPages.has(parentPath)) {
        groupedPages.set(parentPath, new Map());
      }
      
      const pagesInGroup = groupedPages.get(parentPath);
      if (!pagesInGroup?.has(pageKey)) {
        pagesInGroup?.set(pageKey, {
          name: p.pageName,
          url: p.pageUrl,
          operations: [p.operationName]
        });
      } else {
        const pageInfo = pagesInGroup?.get(pageKey);
        if (pageInfo && !pageInfo.operations.includes(p.operationName)) {
          pageInfo.operations.push(p.operationName);
        }
      }
    });

    // Create the hierarchical menu structure
    groupedPages.forEach((pages, parentPath) => {
      if (parentPath === '') {
        // Top-level pages
        pages.forEach((pageInfo, pageName) => {
          if (pageName.toLowerCase() === 'dashboard') return;
          
          if (pageInfo.operations.includes('Read')) {
            menuItems.push({
              icon: this.getIconForPage(pageName),
              label: this.formatLabel(pageName),
              route: pageInfo.url,
              exact: false,
              permission: `${pageName}.Read`
            });
          }
        });
      } else {
        // Create a parent menu item for this group
        const parentMenuItem: MenuItem = {
          icon: this.getIconForParent(parentPath),
          label: this.formatParentLabel(parentPath),
          subItems: []
        };
        
        // Add child items
        pages.forEach((pageInfo, pageName) => {
          if (pageInfo.operations.includes('Read')) {
            parentMenuItem.subItems?.push({
              icon: this.getIconForPage(pageName),
              label: this.formatLabel(pageName),
              route: pageInfo.url,
              exact: false,
              permission: `${pageName}.Read`
            });
          }
        });
        
        // Only add the parent if it has children
        if (parentMenuItem.subItems && parentMenuItem.subItems.length > 0) {
          menuItems.push(parentMenuItem);
        }
      }
    });

    return this.sortMenuItems(menuItems);
  }

  private getParentPath(url: string): string {
    if (!url || !url.includes('/')) return '';
    if (url.startsWith('/')) return '';
    
    const parts = url.split('/');
    return parts[0];
  }
  
  private getIconForParent(parentPath: string): string {
    const parentIconMap: Record<string, string> = {
      'admin': 'admin_panel_settings',
      'settings': 'settings',
      'management': 'manage_accounts'
    };
    
    return parentIconMap[parentPath] || 'folder';
  }
  
  private formatParentLabel(parentPath: string): string {
    return parentPath.charAt(0).toUpperCase() + parentPath.slice(1);
  }

  private getIconForPage(pageName: string): string {
    const iconMap: Record<string, string> = {
      'Users': 'person',
      'Roles': 'admin_panel_settings',
      'Contacts': 'contacts',
      'Permissions': 'security',
      'RolePermissions': 'security',
      'UserRoles': 'manage_accounts',
      'Pages': 'article',
      'Operations': 'list_alt',
      'ActivityLog': 'history',
      'Settings': 'settings'
    };

    return iconMap[pageName] || 'article';
  }

  private formatLabel(pageName: string): string {
    // Add space before each uppercase letter and trim
    return pageName.replace(/([A-Z])/g, ' $1').trim();
  }

  private sortMenuItems(items: MenuItem[]): MenuItem[] {
    const order = ['Dashboard', 'Admin', 'Contacts', 'User', 'Role', 'Permission'];
    
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
