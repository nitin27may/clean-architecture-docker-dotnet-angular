---
layout: default
title: Frontend Guide
nav_order: 6
permalink: /Frontend
---
# Frontend Documentation

## Overview

The frontend of this project is built with Angular 19, leveraging modern Angular features including standalone components, signals for state management, the inject() function for dependency injection, and a powerful combination of Angular Material and TailwindCSS for styling.

<div style="text-align: center; margin: 30px 0;">
  <a href="screenshots/contact-list-page.png" target="_blank">
    <img src="screenshots/contact-list-page.png" alt="Contact List Page" style="width: 90%; max-width: 800px; border-radius: 8px; box-shadow: 0 4px 8px rgba(0,0,0,0.1);">
  </a>
  <p style="font-style: italic; margin-top: 10px;">The Contact List page demonstrating the clean UI design with Angular Material and TailwindCSS</p>
</div>

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

Instead of constructor-based dependency injection, this project uses the modern `inject()` function for cleaner code and better tree-shaking:

```typescript
import { Component, inject, OnInit } from '@angular/core';
import { UserService } from '@core/services/user.service';
import { NotificationService } from '@core/services/notification.service';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-user-profile',
  standalone: true,
  imports: [CommonModule, MatCardModule, MatButtonModule],
  template: `
    <mat-card class="p-4 max-w-md mx-auto">
      <mat-card-header>
        <mat-card-title>{{ user()?.name }}</mat-card-title>
        <mat-card-subtitle>{{ user()?.email }}</mat-card-subtitle>
      </mat-card-header>
      <mat-card-content class="mt-4">
        <p *ngIf="user()">Role: {{ user()?.role }}</p>
      </mat-card-content>
      <mat-card-actions>
        <button mat-raised-button color="primary" (click)="updateProfile()">
          Update Profile
        </button>
      </mat-card-actions>
    </mat-card>
  `
})
export class UserProfileComponent implements OnInit {
  // Modern dependency injection
  private userService = inject(UserService);
  private notificationService = inject(NotificationService);
  
  // Signal-based state
  user = this.userService.currentUser;
  
  ngOnInit() {
    this.loadUserProfile();
  }
  
  loadUserProfile() {
    this.userService.getUserProfile().subscribe({
      next: () => {},
      error: (error) => {
        this.notificationService.showError('Failed to load user profile');
      }
    });
  }
  
  updateProfile() {
    // Profile update logic
  }
}
```

### Signal-Based State Management

The project uses Angular's signals for state management, providing a reactive and efficient way to handle UI state:

```typescript
import { Component, signal, computed, effect } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';

interface Task {
  id: number;
  title: string;
  completed: boolean;
}

@Component({
  selector: 'app-task-manager',
  standalone: true,
  imports: [CommonModule, MatButtonModule, MatCardModule],
  template: `
    <mat-card class="p-4">
      <h2 class="text-xl font-semibold">Task Manager</h2>
      
      <div class="my-3">
        <p>Total Tasks: {{ totalTasks() }}</p>
        <p>Completed: {{ completedCount() }}</p>
        <p>Progress: {{ progressPercentage() }}%</p>
      </div>
      
      <div class="space-y-2">
        <div *ngFor="let task of tasks()" class="flex items-center gap-2">
          <input type="checkbox" [checked]="task.completed" 
                 (change)="toggleTask(task.id)">
          <span [class.line-through]="task.completed">{{ task.title }}</span>
        </div>
      </div>
      
      <button mat-raised-button color="primary" class="mt-4" (click)="addTask()">
        Add Task
      </button>
    </mat-card>
  `
})
export class TaskManagerComponent {
  // Primary signal for state
  tasks = signal<Task[]>([
    { id: 1, title: 'Learn Angular Signals', completed: true },
    { id: 2, title: 'Create a task manager', completed: false },
    { id: 3, title: 'Share with the world', completed: false }
  ]);
  
  // Computed signals for derived state
  totalTasks = computed(() => this.tasks().length);
  
  completedCount = computed(() => 
    this.tasks().filter(task => task.completed).length
  );
  
  progressPercentage = computed(() => {
    if (this.totalTasks() === 0) return 0;
    return Math.round((this.completedCount() / this.totalTasks()) * 100);
  });
  
  // Effect for logging (side effects)
  taskChangeLogger = effect(() => {
    console.log(`Tasks updated. Completed: ${this.completedCount()} of ${this.totalTasks()}`);
  });
  
  toggleTask(id: number) {
    this.tasks.update(tasks => 
      tasks.map(task => 
        task.id === id ? {...task, completed: !task.completed} : task
      )
    );
  }
  
  addTask() {
    const newId = this.tasks().length > 0 
      ? Math.max(...this.tasks().map(t => t.id)) + 1 
      : 1;
      
    this.tasks.update(tasks => [
      ...tasks, 
      { id: newId, title: `New Task ${newId}`, completed: false }
    ]);
  }
}
```

### Theming with Angular Material and TailwindCSS

The application integrates Angular Material's theming system with TailwindCSS for a cohesive design experience that supports both light and dark modes:

```scss
/* styles.scss */
@use "@angular/material" as mat;

// Custom theme configuration
$primary-palette: mat.define-palette(mat.$indigo-palette, 500);
$accent-palette: mat.define-palette(mat.$pink-palette, A200, A100, A400);
$warn-palette: mat.define-palette(mat.$red-palette);

// Light theme
$light-theme: mat.define-light-theme((
  color: (
    primary: $primary-palette,
    accent: $accent-palette,
    warn: $warn-palette,
  ),
  typography: mat.define-typography-config(),
  density: 0,
));

// Dark theme
$dark-theme: mat.define-dark-theme((
  color: (
    primary: $primary-palette,
    accent: $accent-palette,
    warn: $warn-palette,
  ),
  typography: mat.define-typography-config(),
  density: 0,
));

// Apply the light theme by default
@include mat.all-component-themes($light-theme);

// Apply the dark theme when .dark class is present
.dark {
  @include mat.all-component-colors($dark-theme);
}

// TailwindCSS variables that integrate with Material
:root {
  --primary: #{mat.get-color-from-palette($primary-palette, 500)};
  --accent: #{mat.get-color-from-palette($accent-palette, A200)};
  --warn: #{mat.get-color-from-palette($warn-palette, 500)};
}

.dark {
  --primary: #{mat.get-color-from-palette($primary-palette, 300)};
  --accent: #{mat.get-color-from-palette($accent-palette, A100)};
  --warn: #{mat.get-color-from-palette($warn-palette, 300)};
}
```

```typescript
// theme.service.ts
import { Injectable, signal, effect } from '@angular/core';

export type ThemeName = 'light' | 'dark';

@Injectable({
  providedIn: 'root'
})
export class ThemeService {
  // Theme state as a signal
  theme = signal<ThemeName>('light');
  
  constructor() {
    this.initTheme();
    
    // Apply theme changes automatically with an effect
    effect(() => {
      const currentTheme = this.theme();
      if (typeof document !== 'undefined') {
        document.documentElement.classList.toggle('dark', currentTheme === 'dark');
        document.documentElement.style.colorScheme = currentTheme;
      }
    });
  }
  
  private initTheme() {
    // Check user preference from localStorage
    const savedTheme = localStorage.getItem('theme') as ThemeName;
    if (savedTheme) {
      this.theme.set(savedTheme);
      return;
    }
    
    // Check system preference
    if (typeof window !== 'undefined' && window.matchMedia('(prefers-color-scheme: dark)').matches) {
      this.theme.set('dark');
    }
  }
  
  toggleTheme() {
    const newTheme = this.theme() === 'dark' ? 'light' : 'dark';
    this.theme.set(newTheme);
    localStorage.setItem('theme', newTheme);
    return newTheme;
  }
}
```

### Authentication Flow

Authentication is implemented using JWT tokens with automatic refresh capability:

<div style="display: flex; gap: 20px; margin: 20px 0; flex-wrap: wrap;">
  <div style="flex: 1; min-width: 300px;">
    <a href="screenshots/landing-page.png" target="_blank">
      <img src="screenshots/landing-page.png" alt="Login Page" style="width: 100%; border-radius: 8px; box-shadow: 0 4px 8px rgba(0,0,0,0.1);">
    </a>
    <p style="font-style: italic; text-align: center;">Login Page with Material Design components</p>
  </div>
  <div style="flex: 1; min-width: 300px;">
    <a href="screenshots/landing-page-drak.png" target="_blank">
      <img src="screenshots/landing-page-drak.png" alt="Login Page (Dark Mode)" style="width: 100%; border-radius: 8px; box-shadow: 0 4px 8px rgba(0,0,0,0.1);">
    </a>
    <p style="font-style: italic; text-align: center;">Login Page in Dark Mode</p>
  </div>
</div>

```typescript
// auth.service.ts
import { Injectable, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { BehaviorSubject, Observable, of, throwError } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private http = inject(HttpClient);
  private router = inject(Router);
  private jwtHelper = inject(JwtHelperService);
  private apiUrl = '/api/auth';
  
  // Signal for authentication state
  isAuthenticated = signal<boolean>(false);
  
  // Signal for current user's role
  userRole = signal<string | null>(null);
  
  login(credentials: { email: string; password: string }): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/login`, credentials).pipe(
      tap(response => {
        if (response.token) {
          // Store token
          localStorage.setItem('token', response.token);
          
          // Update authentication state using signals
          this.isAuthenticated.set(true);
          
          // Extract and set user role
          const decodedToken = this.jwtHelper.decodeToken(response.token);
          this.userRole.set(decodedToken.role);
        }
      }),
      catchError(err => {
        return throwError(() => new Error('Login failed'));
      })
    );
  }
  
  logout(): void {
    // Clear storage
    localStorage.removeItem('token');
    
    // Update signals
    this.isAuthenticated.set(false);
    this.userRole.set(null);
    
    // Navigate to login
    this.router.navigate(['/login']);
  }
  
  checkAuthStatus(): Observable<boolean> {
    const token = localStorage.getItem('token');
    
    if (!token) {
      this.isAuthenticated.set(false);
      return of(false);
    }
    
    const isTokenExpired = this.jwtHelper.isTokenExpired(token);
    
    if (isTokenExpired) {
      this.logout();
      return of(false);
    }
    
    // Token is valid
    const decodedToken = this.jwtHelper.decodeToken(token);
    this.userRole.set(decodedToken.role);
    this.isAuthenticated.set(true);
    
    return of(true);
  }
}
```

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
│   │   │   ├── user/        # User management
│   │   │   ├── role/        # Role management
│   │   │   ├── permission/  # Permission management
│   │   │   └── settings/    # Application settings
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

<div style="text-align: center; margin: 30px 0;">
  <img src="screenshots/role-permission-mapping.png" alt="Role Permission Mapping" style="width: 90%; max-width: 800px; border-radius: 8px; box-shadow: 0 4px 8px rgba(0,0,0,0.1);">
  <p style="font-style: italic; margin-top: 10px;">Role Permission Mapping interface for administrators</p>
</div>

Security is implemented using a comprehensive permission system:

```typescript
// Permission guard
import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { PermissionService } from '@core/services/permission.service';
import { NotificationService } from '@core/services/notification.service';

export const PermissionGuard = (pageName: string, operation: string = 'Read'): CanActivateFn => {
  return () => {
    const permissionService = inject(PermissionService);
    const router = inject(Router);
    const notificationService = inject(NotificationService);
    
    // Check if user has required permission
    if (permissionService.hasPermission(pageName, operation)) {
      return true;
    }
    
    // Display notification and redirect to home
    notificationService.showError('You do not have permission to access this page');
    router.navigate(['/home']);
    return false;
  };
};

// Permission directive for UI elements
import { Directive, Input, TemplateRef, ViewContainerRef, inject } from '@angular/core';
import { PermissionService } from '@core/services/permission.service';

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

<div style="display: flex; gap: 20px; margin: 20px 0; flex-wrap: wrap;">
  <div style="flex: 1; min-width: 300px;">
    <a href="screenshots/contact-list-page.png" target="_blank">
      <img src="screenshots/contact-list-page.png" alt="Contact List View" style="width: 100%; border-radius: 8px; box-shadow: 0 4px 8px rgba(0,0,0,0.1);">
    </a>
    <p style="font-style: italic; text-align: center;">Contact List View with filtering and sorting</p>
  </div>
  <div style="flex: 1; min-width: 300px;">
    <a href="screenshots/contact-form.png" target="_blank">
      <img src="screenshots/contact-form.png" alt="Contact Form" style="width: 100%; border-radius: 8px; box-shadow: 0 4px 8px rgba(0,0,0,0.1);">
    </a>
    <p style="font-style: italic; text-align: center;">Contact Form with validation</p>
  </div>
</div>

## System Administration

<div style="display: flex; gap: 20px; margin: 20px 0; flex-wrap: wrap;">
  <div style="flex: 1; min-width: 300px;">
    <a href="screenshots/role-master.png" target="_blank">
      <img src="screenshots/role-master.png" alt="Role Management" style="width: 100%; border-radius: 8px; box-shadow: 0 4px 8px rgba(0,0,0,0.1);">
    </a>
    <p style="font-style: italic; text-align: center;">Role Management</p>
  </div>
  <div style="flex: 1; min-width: 300px;">
    <a href="screenshots/operation-master.png" target="_blank">
      <img src="screenshots/operation-master.png" alt="Operation Management" style="width: 100%; border-radius: 8px; box-shadow: 0 4px 8px rgba(0,0,0,0.1);">
    </a>
    <p style="font-style: italic; text-align: center;">Operation Management</p>
  </div>
</div>

## Best Practices

When working with the frontend codebase, follow these guidelines:

1. Create standalone components for better modularity
2. Use signals for component state management
3. Use computed signals for derived state
4. Use the inject() function for dependency injection
5. Leverage TailwindCSS utilities instead of custom CSS where possible
6. Implement proper error handling for API calls
7. Follow the permission model for security
8. Keep components small and focused
9. Use lazy loading for feature modules
10. Maintain accessibility standards

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