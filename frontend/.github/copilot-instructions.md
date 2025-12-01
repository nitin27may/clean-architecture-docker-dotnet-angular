# GitHub Copilot Custom Instructions: Angular 21 + Material 3 + Tailwind 4 Enterprise Admin Dashboard

## Project Overview
This is an enterprise-grade admin dashboard built with Angular 21, Angular Material 3, and Tailwind CSS 4. The application follows modern Angular best practices with standalone components, signals-based state management, and a comprehensive design system that supports responsive layouts and light/dark theming.

---

## CRITICAL RULES - READ FIRST

### 1. Technology Stack (Non-Negotiable)
- **Angular 21**: Use latest features (signals, new control flow, standalone components)
- **Angular Material 19+**: Material Design 3 components only
- **Tailwind CSS 4**: CSS-first configuration (@theme syntax, NO tailwind.config.js)
- **TypeScript 5.5+**: Strict mode enabled

### 2. Framework Hierarchy
```
ALWAYS FOLLOW THIS ORDER:
1. Angular Material → Structure, behavior, accessibility
2. Tailwind 4 → Layout, spacing, responsive utilities
3. Custom CSS → ONLY when absolutely necessary

NEVER override Material component internals with custom CSS.
NEVER duplicate Material functionality with Tailwind.
```

---

## ANGULAR 21 REQUIREMENTS

### Component Architecture

#### ALWAYS Use Standalone Components
```typescript
// ✅ CORRECT
@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, MatCardModule, ...],
  template: `...`
})
export class DashboardComponent {}

// ❌ WRONG - No NgModule components
@NgModule({ ... })
```

#### ALWAYS Use Signals for State Management
```typescript
// ✅ CORRECT - Use signals
export class MyComponent {
  count = signal(0);
  doubleCount = computed(() => this.count() * 2);
  items = signal<Item[]>([]);
  
  increment() {
    this.count.update(n => n + 1);
  }
}

// ❌ WRONG - Don't use traditional properties
export class MyComponent {
  count = 0;  // AVOID
  items: Item[] = [];  // AVOID
}
```

#### ALWAYS Use New Control Flow Syntax
```typescript
// ✅ CORRECT - New @syntax
@Component({
  template: `
    @if (isLoggedIn()) {
      <app-dashboard />
    } @else {
      <app-login />
    }
    
    @for (item of items(); track item.id) {
      <app-item [data]="item" />
    }
    
    @switch (status()) {
      @case ('loading') { <app-loader /> }
      @case ('success') { <app-content /> }
      @case ('error') { <app-error /> }
    }
  `
})

// ❌ WRONG - Old *ng syntax
template: `
  <div *ngIf="isLoggedIn">...</div>  // NEVER USE
  <div *ngFor="let item of items">...</div>  // NEVER USE
  <div [ngSwitch]="status">...</div>  // NEVER USE
`
```

#### ALWAYS Use Input Signals (Angular 17.2+)
```typescript
// ✅ CORRECT - Input signals
export class UserCard {
  userId = input.required<string>();
  userName = input<string>('Guest');
  isActive = input<boolean>(false);
  
  // Computed from inputs
  displayName = computed(() => 
    `${this.userName()} (${this.isActive() ? 'Active' : 'Inactive'})`
  );
}

// ❌ WRONG - Old @Input decorator
export class UserCard {
  @Input() userId!: string;  // AVOID
  @Input() userName = 'Guest';  // AVOID
}
```

#### ALWAYS Use toSignal for Observable Conversion
```typescript
// ✅ CORRECT - Convert observables to signals
export class MyComponent {
  private breakpointObserver = inject(BreakpointObserver);
  
  isMobile = toSignal(
    this.breakpointObserver.observe([Breakpoints.Handset]),
    { initialValue: false }
  );
}

// ❌ WRONG - Subscribe in component
ngOnInit() {
  this.breakpointObserver.observe(...).subscribe(...);  // AVOID
}
```

#### ALWAYS Use OnPush Change Detection
```typescript
// ✅ CORRECT
@Component({
  selector: 'app-my-component',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,  // ALWAYS ADD
  template: `...`
})
```

#### ALWAYS Use inject() Function (Angular 14+)
```typescript
// ✅ CORRECT - inject() in class body
export class MyComponent {
  private router = inject(Router);
  private fb = inject(FormBuilder);
  private myService = inject(MyService);
}

// ❌ WRONG - Constructor injection (legacy style)
constructor(
  private router: Router,
  private fb: FormBuilder
) {}  // AVOID unless necessary
```

---

## TAILWIND 4 CONFIGURATION

### CRITICAL: Tailwind 4 Uses CSS-First Configuration

#### File Structure
```
src/
├── styles/
│   ├── tailwind.css         ← Main Tailwind file with @theme
│   ├── _material-theme.scss ← Material theming
│   └── _utilities.css       ← Custom utilities
└── styles.scss              ← Global styles
```

#### ALWAYS Use @theme in tailwind.css
```css
/* src/styles/tailwind.css */
@import "tailwindcss";

@theme {
  /* CRITICAL: Prefix prevents Material conflicts */
  --prefix: tw;
  
  /* Spacing: 8px grid system */
  --spacing-0: 0;
  --spacing-xs: 0.25rem;   /* 4px */
  --spacing-sm: 0.5rem;    /* 8px - Material base */
  --spacing-md: 1rem;      /* 16px */
  --spacing-lg: 1.5rem;    /* 24px */
  --spacing-xl: 2rem;      /* 32px */
  --spacing-2xl: 3rem;     /* 48px */
  --spacing-3xl: 4rem;     /* 64px */
  
  /* Colors: Sync with Material palette */
  --color-primary-50: #e8eaf6;
  --color-primary-500: #3f51b5;
  --color-primary-900: #1a237e;
  
  --color-accent-500: #e91e63;
  --color-warn-500: #f44336;
  
  --color-neutral-50: #fafafa;
  --color-neutral-100: #f5f5f5;
  --color-neutral-900: #212121;
  
  /* Breakpoints: Match Angular CDK */
  --breakpoint-xs: 0;
  --breakpoint-sm: 600px;
  --breakpoint-md: 960px;
  --breakpoint-lg: 1280px;
  --breakpoint-xl: 1920px;
  
  /* Shadows: Material elevation */
  --shadow-sm: 0 2px 1px -1px rgba(0,0,0,.2), 0 1px 1px 0 rgba(0,0,0,.14), 0 1px 3px 0 rgba(0,0,0,.12);
  --shadow-md: 0 3px 1px -2px rgba(0,0,0,.2), 0 2px 2px 0 rgba(0,0,0,.14), 0 1px 5px 0 rgba(0,0,0,.12);
  --shadow-lg: 0 2px 4px -1px rgba(0,0,0,.2), 0 4px 5px 0 rgba(0,0,0,.14), 0 1px 10px 0 rgba(0,0,0,.12);
}

/* Dark theme overrides */
.dark {
  --color-neutral-50: #212121;
  --color-neutral-900: #fafafa;
}

/* Custom utilities */
@utility elevation-1 {
  box-shadow: var(--shadow-sm);
}
@utility elevation-2 {
  box-shadow: var(--shadow-md);
}
@utility elevation-4 {
  box-shadow: var(--shadow-lg);
}
```

#### NEVER Create tailwind.config.js
```javascript
// ❌ WRONG - Don't create this file in Tailwind 4
// Tailwind 4 doesn't use tailwind.config.js
module.exports = { ... }  // NEVER DO THIS
```

### Tailwind Utility Usage Rules

#### ALWAYS Use tw- Prefix
```html
<!-- ✅ CORRECT -->
<div class="tw-p-md tw-flex tw-gap-sm">

<!-- ❌ WRONG -->
<div class="p-md flex gap-sm">  <!-- Missing tw- prefix -->
```

#### Spacing: ALWAYS Use 8px Grid
```html
<!-- ✅ CORRECT - Use defined spacing scale -->
<div class="tw-p-xs">   <!-- 4px -->
<div class="tw-p-sm">   <!-- 8px -->
<div class="tw-p-md">   <!-- 16px -->
<div class="tw-p-lg">   <!-- 24px -->
<div class="tw-p-xl">   <!-- 32px -->
<div class="tw-p-2xl">  <!-- 48px -->

<!-- ❌ WRONG - Arbitrary values -->
<div class="tw-p-[13px]">  <!-- AVOID arbitrary values -->
<div class="tw-p-3">       <!-- AVOID non-standard spacing -->
```

#### Layout: Use Tailwind for Structure
```html
<!-- ✅ CORRECT - Tailwind for layout -->
<div class="tw-flex tw-items-center tw-justify-between tw-gap-md">
<div class="tw-grid tw-grid-cols-1 md:tw-grid-cols-2 lg:tw-grid-cols-3 tw-gap-lg">
<div class="tw-space-y-md">  <!-- Vertical spacing -->

<!-- ❌ WRONG - Custom CSS for layout -->
<div style="display: flex; gap: 16px;">  <!-- NEVER use inline styles -->
```

---

## ANGULAR MATERIAL 3 GUIDELINES

### Component Selection

#### ALWAYS Use Material for UI Components
```html
<!-- ✅ CORRECT - Use Material components -->
<mat-card>
<mat-toolbar>
<mat-button>
<mat-form-field>
<mat-table>
<mat-sidenav-container>
<mat-icon>
<mat-menu>
<mat-dialog>

<!-- ❌ WRONG - Don't recreate Material components -->
<div class="custom-card">  <!-- Use mat-card instead -->
<button class="custom-button">  <!-- Use mat-button instead -->
```

#### Material Component Appearance
```html
<!-- ✅ CORRECT - Material 3 appearance -->
<mat-form-field appearance="outline">  <!-- ALWAYS use outline -->
  <mat-label>Email</mat-label>
  <input matInput type="email">
  <mat-error>Invalid email</mat-error>
</mat-form-field>

<!-- ❌ WRONG - Legacy appearances -->
<mat-form-field appearance="legacy">  <!-- NEVER use legacy -->
<mat-form-field appearance="fill">    <!-- AVOID fill -->
```

#### Button Hierarchy
```html
<!-- ✅ CORRECT - Proper button hierarchy -->
<button mat-raised-button color="primary">Primary Action</button>
<button mat-flat-button color="accent">Secondary Action</button>
<button mat-stroked-button>Tertiary Action</button>
<button mat-button>Text Action</button>
<button mat-icon-button><mat-icon>close</mat-icon></button>
<button mat-fab color="primary"><mat-icon>add</mat-icon></button>

<!-- ❌ WRONG - Inconsistent styles -->
<button class="tw-bg-primary">Custom Button</button>  <!-- Use Material buttons -->
```

### Theming

#### Material Theme Configuration
```scss
// src/styles/_material-theme.scss
@use '@angular/material' as mat;

@include mat.core();

// Define theme using Material 3 API
$light-theme: mat.define-theme((
  color: (
    theme-type: light,
    primary: mat.$violet-palette,
    tertiary: mat.$pink-palette,
  ),
  typography: (
    brand-family: 'Roboto, sans-serif',
    plain-family: 'Roboto, sans-serif',
  ),
  density: (
    scale: 0
  )
));

$dark-theme: mat.define-theme((
  color: (
    theme-type: dark,
    primary: mat.$violet-palette,
    tertiary: mat.$pink-palette,
  )
));

html {
  @include mat.all-component-themes($light-theme);
}

html.dark {
  @include mat.all-component-colors($dark-theme);
}
```

---

## RESPONSIVE DESIGN RULES

### Mobile-First Approach

#### ALWAYS Design Mobile-First
```html
<!-- ✅ CORRECT - Mobile-first, then larger screens -->
<div class="tw-flex-col md:tw-flex-row">
<div class="tw-w-full lg:tw-w-1/2">
<div class="tw-p-md lg:tw-p-xl">
<div class="tw-hidden md:tw-block">  <!-- Hide on mobile -->
<div class="tw-block md:tw-hidden">  <!-- Show only on mobile -->

<!-- ❌ WRONG - Desktop-first -->
<div class="tw-flex-row md:tw-flex-col">  <!-- Backwards -->
```

### Breakpoint Usage

#### Standard Breakpoints (Match Angular CDK)
```
xs: 0px      - Extra small (mobile portrait)
sm: 600px    - Small (mobile landscape)
md: 960px    - Medium (tablet)
lg: 1280px   - Large (desktop)
xl: 1920px   - Extra large (large desktop)
```

#### Responsive Patterns
```html
<!-- ✅ Grid Columns -->
<div class="tw-grid tw-grid-cols-1 sm:tw-grid-cols-2 lg:tw-grid-cols-3 xl:tw-grid-cols-4">

<!-- ✅ Flex Direction -->
<div class="tw-flex tw-flex-col md:tw-flex-row">

<!-- ✅ Spacing Adjustments -->
<div class="tw-p-sm md:tw-p-md lg:tw-p-lg xl:tw-p-xl">

<!-- ✅ Text Sizing -->
<h1 class="tw-text-xl md:tw-text-2xl lg:tw-text-3xl">

<!-- ✅ Visibility -->
<div class="tw-hidden md:tw-block">Desktop Only</div>
<div class="tw-block md:tw-hidden">Mobile Only</div>
```

#### Responsive Layout Component
```typescript
// ✅ CORRECT - Use CDK BreakpointObserver
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';

export class LayoutComponent {
  private breakpointObserver = inject(BreakpointObserver);
  
  isMobile = toSignal(
    this.breakpointObserver.observe([Breakpoints.Handset]),
    { initialValue: false }
  );
  
  isTablet = toSignal(
    this.breakpointObserver.observe([Breakpoints.Tablet]),
    { initialValue: false }
  );
  
  isDesktop = toSignal(
    this.breakpointObserver.observe([Breakpoints.Web]),
    { initialValue: false }
  );
}
```

### Touch Targets

#### ALWAYS Ensure Minimum 44x44px Touch Targets
```html
<!-- ✅ CORRECT - Adequate touch target -->
<button mat-icon-button class="tw-w-11 tw-h-11">  <!-- 44px minimum -->
  <mat-icon>menu</mat-icon>
</button>

<!-- ❌ WRONG - Too small for touch -->
<button class="tw-w-6 tw-h-6">  <!-- 24px - too small -->
```

---

## DARK THEME SUPPORT

### Theme Toggle Implementation

#### Component with Theme Switching
```typescript
// ✅ CORRECT - Theme toggle with signal
@Component({
  selector: 'app-layout',
  standalone: true,
  template: `
    <button mat-icon-button (click)="toggleTheme()">
      <mat-icon>{{ isDark() ? 'light_mode' : 'dark_mode' }}</mat-icon>
    </button>
  `
})
export class LayoutComponent {
  isDark = signal(false);
  
  constructor() {
    // Load saved theme preference
    const savedTheme = localStorage.getItem('theme');
    if (savedTheme === 'dark') {
      this.isDark.set(true);
      document.documentElement.classList.add('dark');
    }
  }
  
  toggleTheme() {
    this.isDark.update(dark => !dark);
    document.documentElement.classList.toggle('dark');
    localStorage.setItem('theme', this.isDark() ? 'dark' : 'light');
  }
}
```

### Dark Mode Class Usage

#### ALWAYS Use dark: Variants
```html
<!-- ✅ CORRECT - Light and dark variants -->
<div class="tw-bg-neutral-50 dark:tw-bg-neutral-900">
<div class="tw-text-neutral-900 dark:tw-text-neutral-50">
<div class="tw-border-neutral-200 dark:tw-border-neutral-700">
<div class="hover:tw-bg-neutral-100 dark:hover:tw-bg-neutral-800">

<!-- ❌ WRONG - No dark variants -->
<div class="tw-bg-white tw-text-black">  <!-- Won't work in dark mode -->
```

#### Material Components Auto-Adapt
```html
<!-- ✅ CORRECT - Material handles theming automatically -->
<mat-card>  <!-- Automatically themed -->
<mat-toolbar color="primary">  <!-- Uses theme colors -->
<button mat-raised-button color="accent">  <!-- Theme-aware -->

<!-- No need to add dark: classes to Material components -->
```

---

## LAYOUT PATTERNS

### Standard Admin Layout
```typescript
@Component({
  selector: 'app-layout',
  standalone: true,
  imports: [MatSidenavModule, MatToolbarModule, MatButtonModule, MatIconModule],
  template: `
    <mat-sidenav-container class="tw-h-screen">
      <!-- Sidebar -->
      <mat-sidenav
        #sidenav
        [mode]="isMobile() ? 'over' : 'side'"
        [opened]="!isMobile()"
        class="tw-w-64 tw-border-r tw-border-neutral-200 dark:tw-border-neutral-700">
        
        <div class="tw-p-lg">
          <h2 class="tw-text-xl tw-font-semibold tw-mb-md">Admin Panel</h2>
          <nav class="tw-space-y-sm">
            <a mat-button class="tw-w-full tw-justify-start">
              <mat-icon class="tw-mr-sm">dashboard</mat-icon>
              Dashboard
            </a>
          </nav>
        </div>
      </mat-sidenav>

      <!-- Main Content -->
      <mat-sidenav-content class="tw-flex tw-flex-col">
        <!-- Header -->
        <mat-toolbar class="elevation-1 tw-sticky tw-top-0 tw-z-[1000]">
          @if (isMobile()) {
            <button mat-icon-button (click)="sidenav.toggle()">
              <mat-icon>menu</mat-icon>
            </button>
          }
          <span class="tw-flex-1">{{ pageTitle() }}</span>
          <button mat-icon-button (click)="toggleTheme()">
            <mat-icon>{{ isDark() ? 'light_mode' : 'dark_mode' }}</mat-icon>
          </button>
        </mat-toolbar>

        <!-- Page Content -->
        <main class="tw-flex-1 tw-p-md md:tw-p-lg lg:tw-p-xl tw-bg-neutral-50 dark:tw-bg-neutral-900">
          <router-outlet />
        </main>

        <!-- Footer -->
        <footer class="tw-p-md tw-border-t tw-border-neutral-200 dark:tw-border-neutral-700">
          <p class="tw-text-center tw-text-sm tw-text-neutral-600 dark:tw-text-neutral-400">
            © 2024 Your Company
          </p>
        </footer>
      </mat-sidenav-content>
    </mat-sidenav-container>
  `
})
```

### Card Layout Pattern
```html
<!-- ✅ CORRECT - Consistent card structure -->
<mat-card class="elevation-2">
  <mat-card-header class="tw-p-lg tw-border-b tw-border-neutral-200 dark:tw-border-neutral-700">
    <mat-card-title class="tw-text-2xl tw-m-0">Title</mat-card-title>
    <mat-card-subtitle class="tw-mt-xs">Subtitle</mat-card-subtitle>
  </mat-card-header>

  <mat-card-content class="tw-p-lg">
    Content goes here
  </mat-card-content>

  <mat-card-actions class="tw-p-lg tw-border-t tw-border-neutral-200 dark:tw-border-neutral-700 tw-flex tw-gap-sm tw-justify-end">
    <button mat-button>Cancel</button>
    <button mat-raised-button color="primary">Save</button>
  </mat-card-actions>
</mat-card>
```

### Grid/Table Pattern
```typescript
@Component({
  template: `
    <mat-card class="tw-overflow-hidden elevation-2">
      <mat-card-header class="tw-p-lg tw-border-b tw-border-neutral-200 dark:tw-border-neutral-700">
        <div class="tw-flex tw-items-center tw-justify-between tw-w-full">
          <mat-card-title class="tw-text-2xl tw-m-0">Data Grid</mat-card-title>
          <button mat-raised-button color="primary">
            <mat-icon>add</mat-icon>
            Add New
          </button>
        </div>
      </mat-card-header>

      <div class="tw-overflow-x-auto">
        <table mat-table [dataSource]="data()" class="tw-w-full">
          <ng-container matColumnDef="name">
            <th mat-header-cell *matHeaderCellDef class="tw-font-semibold">Name</th>
            <td mat-cell *matCellDef="let row" class="tw-py-md">{{ row.name }}</td>
          </ng-container>

          <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
          <tr mat-row *matRowDef="let row; columns: displayedColumns;"
              class="hover:tw-bg-neutral-100 dark:hover:tw-bg-neutral-800 tw-transition-colors">
          </tr>
        </table>
      </div>

      <mat-paginator [pageSizeOptions]="[10, 25, 50]" />
    </mat-card>
  `
})
```

### Form Pattern
```typescript
@Component({
  template: `
    <mat-card class="tw-max-w-2xl tw-mx-auto elevation-2">
      <mat-card-header class="tw-p-lg tw-border-b tw-border-neutral-200 dark:tw-border-neutral-700">
        <mat-card-title class="tw-text-2xl tw-m-0">Form Title</mat-card-title>
      </mat-card-header>

      <mat-card-content class="tw-p-lg">
        <form [formGroup]="form" class="tw-space-y-md">
          <mat-form-field appearance="outline" class="tw-w-full">
            <mat-label>Field Label</mat-label>
            <input matInput formControlName="field">
            <mat-error>This field is required</mat-error>
          </mat-form-field>
          
          <!-- More fields -->
        </form>
      </mat-card-content>

      <mat-card-actions class="tw-p-lg tw-border-t tw-border-neutral-200 dark:tw-border-neutral-700 tw-flex tw-gap-sm tw-justify-end">
        <button mat-button type="button">Cancel</button>
        <button mat-raised-button color="primary" [disabled]="form.invalid">
          Save
        </button>
      </mat-card-actions>
    </mat-card>
  `
})
```

---

## ACCESSIBILITY REQUIREMENTS

### ARIA Labels
```html
<!-- ✅ CORRECT - Proper ARIA labels -->
<button mat-icon-button aria-label="Open menu">
  <mat-icon>menu</mat-icon>
</button>

<mat-form-field>
  <mat-label>Email</mat-label>
  <input matInput type="email" aria-label="Email address">
</mat-form-field>

<!-- ❌ WRONG - Missing labels -->
<button mat-icon-button>  <!-- Missing aria-label -->
  <mat-icon>menu</mat-icon>
</button>
```

### Keyboard Navigation
```typescript
// ✅ CORRECT - Keyboard event handlers
@Component({
  template: `
    <div (keydown.enter)="onActivate()" (keydown.space)="onActivate()" tabindex="0">
      Clickable div
    </div>
  `
})
```

### Focus Management
```typescript
// ✅ CORRECT - Manage focus properly
export class DialogComponent implements AfterViewInit {
  @ViewChild('closeButton') closeButton!: ElementRef;
  
  ngAfterViewInit() {
    // Focus first interactive element
    this.closeButton.nativeElement.focus();
  }
}
```

---

## PERFORMANCE BEST PRACTICES

### Change Detection
```typescript
// ✅ ALWAYS use OnPush
@Component({
  selector: 'app-my-component',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `...`
})
```

### Lazy Loading
```typescript
// ✅ CORRECT - Lazy load routes
const routes: Routes = [
  {
    path: 'admin',
    loadComponent: () => import('./admin/admin.component').then(m => m.AdminComponent)
  },
  {
    path: 'dashboard',
    loadChildren: () => import('./dashboard/routes').then(m => m.DASHBOARD_ROUTES)
  }
];
```

### Track By Functions
```html
<!-- ✅ CORRECT - Use track in @for -->
@for (item of items(); track item.id) {
  <app-item [data]="item" />
}

<!-- ❌ WRONG - No tracking -->
@for (item of items(); track $index) {  <!-- Less efficient -->
  <app-item [data]="item" />
}
```

---

## CODE QUALITY STANDARDS

### TypeScript Strict Mode
```json
// tsconfig.json - ALWAYS enable strict
{
  "compilerOptions": {
    "strict": true,
    "noImplicitAny": true,
    "strictNullChecks": true,
    "strictFunctionTypes": true,
    "strictPropertyInitialization": true,
    "noImplicitThis": true,
    "alwaysStrict": true
  }
}
```

### Type Safety
```typescript
// ✅ CORRECT - Proper typing
interface User {
  id: string;
  name: string;
  email: string;
  role: 'admin' | 'user' | 'guest';
}

export class UserComponent {
  users = signal<User[]>([]);
  selectedUser = signal<User | null>(null);
}

// ❌ WRONG - Any types
users = signal<any[]>([]);  // NEVER use any
selectedUser = signal<any>(null);  // NEVER use any
```

### Naming Conventions
```typescript
// Components: PascalCase + Component suffix
export class UserDashboardComponent {}

// Services: PascalCase + Service suffix
export class AuthService {}

// Interfaces: PascalCase (no I prefix)
export interface User {}  // ✅ CORRECT
export interface IUser {}  // ❌ WRONG

// Signals: camelCase
const userName = signal('');
const isLoading = signal(false);

// Constants: UPPER_SNAKE_CASE
const MAX_RETRIES = 3;
const API_BASE_URL = 'https://api.example.com';
```

---

## FILE ORGANIZATION

### Project Structure
```
src/
├── app/
│   ├── core/                    # Singleton services
│   │   ├── auth/
│   │   ├── interceptors/
│   │   └── guards/
│   ├── shared/                  # Shared components
│   │   ├── components/
│   │   │   ├── data-grid/
│   │   │   ├── form-card/
│   │   │   └── page-header/
│   │   ├── directives/
│   │   ├── pipes/
│   │   └── models/
│   ├── features/                # Feature modules
│   │   ├── dashboard/
│   │   ├── users/
│   │   └── settings/
│   └── layout/                  # Layout components
│       ├── header/
│       ├── sidebar/
│       └── footer/
├── styles/
│   ├── tailwind.css            # Tailwind 4 @theme
│   ├── _material-theme.scss    # Material theming
│   └── _utilities.css          # Custom utilities
└── assets/
```

---

## ERROR HANDLING

### HTTP Error Handling
```typescript
// ✅ CORRECT - Proper error handling
export class DataService {
  private http = inject(HttpClient);
  
  getData() {
    return this.http.get<Data[]>('/api/data').pipe(
      catchError(error => {
        console.error('Error fetching data:', error);
        return of([]);  // Return empty array as fallback
      })
    );
  }
}
```

### Form Validation
```typescript
// ✅ CORRECT - Comprehensive validation
export class FormComponent {
  private fb = inject(FormBuilder);
  
  form = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(8)]],
    confirmPassword: ['', Validators.required]
  }, {
    validators: this.passwordMatchValidator
  });
  
  passwordMatchValidator(group: AbstractControl): ValidationErrors | null {
    const password = group.get('password')?.value;
    const confirmPassword = group.get('confirmPassword')?.value;
    return password === confirmPassword ? null : { passwordMismatch: true };
  }
}
```

---

## TESTING GUIDELINES

### Component Testing
```typescript
// ✅ CORRECT - Test with signals
describe('UserComponent', () => {
  it('should update user count', () => {
    const component = new UserComponent();
    component.users.set([{ id: '1', name: 'John' }]);
    expect(component.userCount()).toBe(1);
  });
});
```

---

## COMMON ANTI-PATTERNS TO AVOID

### ❌ DON'T: Override Material Styles
```css
/* WRONG - Never do this */
.mat-button {
  background: red !important;  /* Don't override Material */
}
```

### ❌ DON'T: Use Inline Styles
```html
<!-- WRONG -->
<div style="padding: 16px; display: flex;">  <!-- Use Tailwind -->
```

### ❌ DON'T: Hardcode Colors
```html
<!-- WRONG -->
<div class="tw-bg-[#3f51b5]">  <!-- Use theme colors -->

<!-- CORRECT -->
<div class="tw-bg-primary-500">
```

### ❌ DON'T: Use Traditional RxJS Subscriptions
```typescript
// WRONG
ngOnInit() {
  this.dataService.getData().subscribe(data => {
    this.data = data;  // Manual subscription management
  });
}

// CORRECT - Use toSignal
data = toSignal(this.dataService.getData(), { initialValue: [] });
```

### ❌ DON'T: Use ViewChild for Data Access
```typescript
// WRONG
@ViewChild('table') table: MatTable;
ngAfterViewInit() {
  const data = this.table.dataSource;  // Avoid ViewChild for data
}

// CORRECT - Use signals
tableData = signal<Data[]>([]);
```

---

## FINAL CHECKLIST FOR EVERY COMPONENT

- [ ] Standalone component with imports
- [ ] OnPush change detection
- [ ] Signals for all state
- [ ] New control flow syntax (@if, @for)
- [ ] Input signals for @Input
- [ ] inject() for dependency injection
- [ ] Tailwind classes use tw- prefix
- [ ] Spacing follows 8px grid (tw-p-sm/md/lg)
- [ ] Material components for UI
- [ ] Dark mode variants (dark:tw-*)
- [ ] Responsive breakpoints (sm:, md:, lg:)
- [ ] ARIA labels on interactive elements
- [ ] Proper TypeScript typing (no any)
- [ ] Track by in @for loops

---

## SUMMARY: KEY PRINCIPLES

1. **Angular 21 First**: Use latest features (signals, new control flow, standalone)
2. **Material for Components**: Never recreate what Material provides
3. **Tailwind for Layout**: Use tw- prefix, follow 8px grid
4. **Mobile-First**: Design for mobile, enhance for desktop
5. **Dark Theme Always**: Every component must work in light AND dark
6. **Accessibility Built-In**: ARIA labels, keyboard nav, focus management
7. **Type Safety**: Strict TypeScript, no any types
8. **Performance**: OnPush, lazy loading, proper change detection

---

## WHEN IN DOUBT

1. Check if Material has the component → Use Material
2. Need layout/spacing → Use Tailwind with tw- prefix
3. Need state → Use signals
4. Need control flow → Use @if/@for/@switch
5. Need responsive → Mobile-first with sm:/md:/lg:
6. Need theming → Use theme colors + dark: variants

**Remember**: Material handles the component, Tailwind handles the layout, Angular handles the logic.

---

*This file is the source of truth for all code generation. Follow these rules strictly for consistency and maintainability.*