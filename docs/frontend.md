# Frontend Documentation

## Overview

The frontend of this project is built with Angular 19, leveraging modern Angular features including signals, the inject() function for dependency injection, and a combination of Angular Material and TailwindCSS for styling.

## Technology Stack

- **Angular 19**
  - Modern dependency injection with `inject()`
  - Signal-based state management
  - Reactive programming patterns
  - Lazy loading for optimized bundles

- **Angular Material 19**
  - Comprehensive UI component library
  - Theme customization
  - Accessibility features
  - Responsive components

- **TailwindCSS v4**
  - Utility-first CSS framework
  - Custom configuration for project-specific design
  - Integration with Material theming
  - JIT compiler for optimized CSS

- **Additional Libraries**
  - RxJS for reactive programming
  - Angular JWT for token handling
  - NgRx (optional) for complex state management

## Key Architectural Patterns

### Modern Dependency Injection

Instead of constructor-based dependency injection, this project uses the modern `inject()` function:

```typescript
// Modern approach with inject()
import { Component, inject } from '@angular/core';
import { UserService } from './user.service';

@Component({
  selector: 'app-user-profile',
  template: '...'
})
export class UserProfileComponent {
  private userService = inject(UserService);
  
  // Component logic using the injected service
}
```

### Signal-Based State Management

The project uses signals for state management, providing a reactive and efficient way to handle UI state:

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

### Authentication Flow

Authentication is implemented using JWT tokens with automatic refresh capability:

1. User logs in via login form
2. JWT token is stored securely
3. HTTP interceptor adds token to all API requests
4. Token refresh is handled automatically before expiration
5. Auth guard protects routes based on roles and permissions

## Project Structure

```
/frontend
├── src/
│   ├── app/
│   │   ├── core/           # Core functionality (auth, guards, interceptors)
│   │   ├── features/       # Feature modules (user, contact, etc.)
│   │   ├── shared/         # Shared components, directives, pipes
│   │   └── app.component.* # Root component files
│   ├── assets/             # Static assets
│   ├── environments/       # Environment configurations
│   └── styles/             # Global styles
├── Dockerfile              # Production Docker configuration
├── debug.dockerfile        # Development Docker configuration
└── package.json            # Dependencies and scripts
```

## Key Features

### User Authentication

The application includes a complete authentication system with:

- Login with JWT token
- Registration with validation
- Forgot password workflow
- Password change functionality
- Profile management

### Role-Based Access Control

UI elements and routes are conditionally displayed based on user roles:

- Route guards for protected pages
- Conditional UI rendering based on permissions
- API access restrictions handled by interceptors

### Contact Management (CRUD Example)

Complete CRUD functionality is implemented for contacts:

- Contact listing with sorting and filtering
- Contact creation form
- Contact editing
- Contact deletion with confirmation
- Responsive design for all device sizes

## Development Workflow

See the [Development Guide](./development-guide.md) for detailed information on working with the frontend codebase.

## Docker Integration

The frontend is containerized with two Docker configurations:

1. **Production Build**: Optimized for production deployment
   - Multi-stage build for minimal image size
   - AOT compilation and bundle optimization
   - NGINX for serving static assets

2. **Development Build**: Configured for active development
   - Live reload functionality
   - Source maps for debugging
   - Development server with hot module replacement

## Styling Guide

The project uses a combination of Angular Material components and TailwindCSS utilities:

- Material components for complex UI elements (forms, tables, dialogs)
- TailwindCSS for layout, spacing, and custom styling
- Custom Material theme with primary, accent, and warn colors
- Dark/light mode support

## Best Practices

When working with the frontend codebase, follow these guidelines:

1. Use signals for component state
2. Use computed signals for derived state
3. Use the inject() function for dependency injection
4. Leverage TailwindCSS utilities instead of custom CSS where possible
5. Implement lazy loading for feature modules
6. Follow Angular's OnPush change detection strategy
7. Implement proper error handling for API calls
8. Use typed forms for form handling
