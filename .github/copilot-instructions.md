# GitHub Copilot Instructions for Angular

Generate code following these modern Angular 19+ best practices:

## Dependency Injection
- Always use functional injection with `inject()` instead of constructor-based injection
- Example: `private userService = inject(UserService);` instead of constructor injection
- Apply this for services, tokens, and parent component references

## State Management
- Use signals for component state instead of class properties
  - Example: `counter = signal<number>(0);`
- Use computed signals for derived state
  - Example: `doubleCounter = computed(() => this.counter() * 2);`
- Prefer signals over RxJS observables for component-level state

## Component Structure
- Use standalone components as the default approach
- Use the modern control flow syntax (`@if`, `@for`, etc.)
- Implement OnPush change detection strategy for better performance
- Always use separte html and scss file for view and styling

## Services & HTTP
- For service calls using subscribe, use the next syntax:
  this.userService.getUsers().subscribe({
    next: (users) => this.users.set(users),
    error: (error) => this.errorMessage.set(error)
  });

## Routing & Guards
- Use functional syntax for guards and resolvers
- Example:
  export const authGuard = () => {
    const router = inject(Router);
    const authService = inject(AuthService);
    
    if (authService.isAuthenticated()) {
      return true;
    }
    
    return router.parseUrl('/login');
  };

## HTTP Interceptors
- Use functional interceptors
- Example:
  export const authInterceptor: HttpInterceptorFn = (req, next) => {
    const authService = inject(AuthService);
    const token = authService.getToken();
    
    if (token) {
      const authReq = req.clone({
        headers: req.headers.set('Authorization', `Bearer ${token}`)
      });
      return next(authReq);
    }
    
    return next(req);
  };

## Styling
- Use Angular Material 19 with proper theming
- Implement TailwindCSS v4 for utility-first styling
- Ensure components support theme switching
- Apply consistent color tokens from design system

## Input/Output Handling
- Use strongly typed inputs and outputs
- Example:
  @Input({ required: true }) id!: string;
  @Output() saved = new EventEmitter<User>();