# Frontend Documentation

## Overview

The frontend of this project is built with Angular 19, leveraging modern Angular features including standalone components, signals for state management, the inject() function for dependency injection, and a powerful combination of Angular Material and TailwindCSS for styling.

## Technology Stack

- **Angular 19**
  - Standalone components architecture
  - Modern dependency injection with `inject()`
  - Signal-based state management
  - Reactive programming with RxJS
  - Lazy-loaded routes for optimized performance

- **Angular Material 19**
  - Comprehensive UI component library
  - Custom theme configuration
  - Dark mode support
  - Accessibility features

- **TailwindCSS v4**
  - Utility-first CSS framework
  - Integration with Material Design
  - Custom color schemes
  - Responsive design utilities

- **Additional Libraries**
  - RxJS for reactive programming
  - Angular JWT for token handling

## Key Architectural Patterns

### Standalone Components

The application uses the standalone component pattern for better modularity and tree-shaking:

```typescript
import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-example',
  standalone: true,
  imports: [
    CommonModule,
    MatButtonModule,
    RouterModule
  ],
  template: `
    <button mat-raised-button color="primary" routerLink="/dashboard">
      Dashboard
    </button>
  `
})
export class ExampleComponent {}
```

### Modern Dependency Injection

Instead of constructor-based dependency injection, this project uses the modern `inject()` function:

```typescript
import { Component, inject } from '@angular/core';
import { UserService } from '@core/services/user.service';

@Component({
  selector: 'app-user-profile',
  standalone: true,
  template: '...'
})
export class UserProfileComponent {
  private userService = inject(UserService);
  
  // Component logic using the injected service
}
```

### Signal-Based State Management

The project uses Angular's signals for state management, providing a reactive and efficient way to handle UI state:

```typescript
import { Component, signal, computed } from '@angular/core';

@Component({
  selector: 'app-counter',
  template: `
    <div>Count: {{ count() }}</div>
    <div>Double count: {{ doubleCount() }}</div>
    <button (click)="increment()">Increment</button>
  `
})
export class CounterComponent {
  // Signal for state
  count = signal<number>(0);
  
  // Computed signal for derived state
  doubleCount = computed(() => this.count() * 2);
  
  increment() {
    this.count.update(value => value + 1);
  }
}
```

### Theming with Angular Material and TailwindCSS

The application integrates Angular Material's theming system with TailwindCSS for a cohesive design experience:

```scss
@use "@angular/material" as mat;

html {
  color-scheme: light;
  @include mat.theme(
    (
      color: mat.$azure-palette,
      typography: Inter,
    )
  );
}

.dark {
  color-scheme: dark;
}
```

### Authentication Flow

Authentication is implemented using JWT tokens with automatic refresh capability:

1. User logs in via login form
2. JWT token is stored securely
3. HTTP interceptor adds token to all API requests
4. Auth guard protects routes based on roles and permissions

## Project Structure

```
/frontend
├── src/
│   ├── app/
│   │   ├── @core/           # Core functionality
│   │   │   ├── components/  # Shared components
│   │   │   ├── directives/  # Custom directives
│   │   │   ├── guards/      # Route guards
│   │   │   ├── interceptors/# HTTP interceptors
│   │   │   ├── layout/      # Layout components
│   │   │   ├── models/      # Interfaces and types
│   │   │   ├── pipes/       # Custom pipes
│   │   │   └── services/    # Global services
│   │   ├── feature/         # Feature modules
│   │   │   ├── contact/     # Contact management
│   │   │   └── user/        # User management
│   │   ├── environments/    # Environment config
│   │   └── styles/          # Global styles
│   ├── assets/              # Static assets
│   └── index.html           # Main HTML entry
├── Dockerfile               # Production container
├── debug.dockerfile         # Development container
└── package.json             # Dependencies
```

## Key Features

### User Authentication

The application includes a complete authentication system with:

- Login with JWT token
- Registration with validation
- Role-based permissions
- Profile management

### Role-Based Access Control

Security is implemented using a comprehensive permission system:

```typescript
// Permission guard
export const PermissionGuard = (pageName: string, operation: string = 'Read'): CanActivateFn => {
  return () => {
    const permissionService = inject(PermissionService);
    // Check if user has required permission
    if (permissionService.hasPermission(pageName, operation)) {
      return true;
    }
    return false;
  };
};

// Permission directive for UI elements
@Directive({
  selector: '[hasPermission]',
  standalone: true
})
export class HasPermissionDirective {
  private permissionService = inject(PermissionService);
  private viewContainer = inject(ViewContainerRef);
  private templateRef = inject(TemplateRef<any>);

  @Input() set hasPermission(permission: { page: string; operation: string }) {
    if (this.permissionService.hasPermission(permission.page, permission.operation)) {
      this.viewContainer.createEmbeddedView(this.templateRef);
    } else {
      this.viewContainer.clear();
    }
  }
}
```

### Contact Management (CRUD Example)

Full CRUD operations for contacts with:

- List view with filtering
- Detail view
- Create/Edit forms
- Delete confirmation
- Permission checks

### Dark/Light Theme Support

The application supports dynamic theme switching with seamless integration between Material and Tailwind:

```typescript
export class ThemeService {
  // Theme state
  private appTheme = signal<ThemeName>('light');
  
  // Toggle theme
  toggleTheme() {
    const newTheme = this.appTheme() === 'dark' ? 'light' : 'dark';
    this.setTheme(newTheme);
    return newTheme;
  }
  
  // Apply theme to DOM
  private applyThemeEffect = effect(() => {
    const theme = this.appTheme();
    if (typeof document !== 'undefined') {
      if (theme === 'dark') {
        document.documentElement.classList.add('dark');
      } else {
        document.documentElement.classList.remove('dark');
      }
      document.documentElement.style.colorScheme = theme;
    }
  });
}
```

## Development Workflow

### Starting the Frontend

Using Docker (recommended):
```bash
docker-compose up frontend
```

Locally:
```bash
cd frontend
npm install
npm start
```

### Building for Production

```bash
npm run build
```

Or using Docker:
```bash
docker build -f Dockerfile -t contact-frontend .
```

### Testing

```bash
npm test
```

## Best Practices

When working with the frontend codebase, follow these guidelines:

1. Create standalone components
2. Use signals for component state
3. Use computed signals for derived state
4. Use the inject() function for dependency injection
5. Leverage TailwindCSS utilities instead of custom CSS where possible
6. Implement proper error handling for API calls
7. Follow the permission model for security
8. Keep components small and focused
9. Use lazy loading for feature modules
10. Maintain accessibility standards