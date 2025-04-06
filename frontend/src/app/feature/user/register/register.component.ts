import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { signal, computed, inject } from '@angular/core';
import { UserService } from '@core/services/user.service';
import { RouterModule, Router } from "@angular/router";
import { CommonModule } from "@angular/common";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatInputModule } from "@angular/material/input";
import { MatIconModule } from "@angular/material/icon";
import { MatButtonModule } from "@angular/material/button";
import { MatProgressSpinnerModule } from "@angular/material/progress-spinner";
import { MatCheckboxModule } from '@angular/material/checkbox';
import { errorTailorImports } from '@core/components/validation';
import { NotificationService } from '@core/services/notification.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
  standalone: true,
  imports: [
    RouterModule,
    ReactiveFormsModule,
    CommonModule,
    MatFormFieldModule,
    MatInputModule,
    MatIconModule,
    MatButtonModule,
    MatProgressSpinnerModule,
    MatCheckboxModule,
    errorTailorImports
  ],
})
export class RegisterComponent {
  // Inject dependencies
  private fb = inject(FormBuilder);
  private userService = inject(UserService);
  private notificationService = inject(NotificationService);
  private router = inject(Router);

  // State with signals
  hidePassword = signal<boolean>(true);
  loading = signal<boolean>(false);

  // Form with validation
  registerForm: FormGroup = this.fb.group({
    firstName: ['', [Validators.required, Validators.minLength(2)]],
    lastName: ['', [Validators.required, Validators.minLength(2)]],
    email: ['', [Validators.required, Validators.email]],
    password: ['', [
      Validators.required,
      Validators.minLength(8),
      Validators.pattern(/^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$/)
    ]],
    confirmPassword: ['', Validators.required],
    terms: [false, Validators.requiredTrue]
  }, { validators: this.passwordMatchValidator });

  // Form error getters
  getFirstNameError = computed(() => {
    const control = this.registerForm.get('firstName');
    if (control?.hasError('required')) return 'First name is required';
    if (control?.hasError('minlength')) return 'First name must be at least 2 characters';
    return '';
  });

  getLastNameError = computed(() => {
    const control = this.registerForm.get('lastName');
    if (control?.hasError('required')) return 'Last name is required';
    if (control?.hasError('minlength')) return 'Last name must be at least 2 characters';
    return '';
  });

  getEmailError = computed(() => {
    const control = this.registerForm.get('email');
    if (control?.hasError('required')) return 'Email is required';
    if (control?.hasError('email')) return 'Please enter a valid email';
    return '';
  });

  getPasswordError = computed(() => {
    const control = this.registerForm.get('password');
    if (control?.hasError('required')) return 'Password is required';
    if (control?.hasError('minlength')) return 'Password must be at least 8 characters';
    if (control?.hasError('pattern')) return 'Password must include letters, numbers, and special characters';
    return '';
  });

  getConfirmPasswordError = computed(() => {
    const control = this.registerForm.get('confirmPassword');
    if (control?.hasError('required')) return 'Please confirm your password';
    if (this.registerForm.hasError('passwordMismatch')) return 'Passwords do not match';
    return '';
  });

  getTermsError = computed(() => {
    const control = this.registerForm.get('terms');
    if (control?.hasError('required')) return 'You must accept the terms to register';
    return '';
  });

  // Toggle password visibility
  togglePasswordVisibility(): void {
    this.hidePassword.update(value => !value);
  }

  // Password match validator
  private passwordMatchValidator(g: FormGroup) {
    return g.get('password')?.value === g.get('confirmPassword')?.value
      ? null : { passwordMismatch: true };
  }

  // Submit handler
  onSubmit(): void {
    if (this.registerForm.valid) {
      this.loading.set(true);

      // Create user and handle response
      this.userService.create(this.registerForm.value).subscribe({
        next: (response) => {
          this.loading.set(false);
          this.notificationService.success('Registration successful! Please log in.');
          this.router.navigate(['/login']);
        },
        error: (error) => {
          this.loading.set(false);
          const errorMessage = error?.error?.message || 'Registration failed. Please try again.';
          this.notificationService.error(errorMessage);
        }
      });
    } else {
      // Mark all fields as touched to show validation errors
      Object.keys(this.registerForm.controls).forEach(key => {
        this.registerForm.get(key)?.markAsTouched();
      });
    }
  }
}
