# Use modern dependency injection syntax

- Whenever you have to inject a dependency using Angular DI system, use the `inject` function instead of constructor based injection.
   - this will apply to injecting services, tokens and parent components e.g. in directives as well



# For all state in components and directives, use a signal instead of a class property
- For example:

` counter = signal<number>(0); `

- For derived state from an existing signal, use computeds as follows.

` doubleCounter = computed(() => this.counter() * 2); `


- Use function style for guard, interceptors

- for the service call if using subscribe use  next: 

# For styling use Angular Material 19 with theming and tailwind v4 
 - TailwindCSS for utility-first styling
 - make sure everything supporting the theme
