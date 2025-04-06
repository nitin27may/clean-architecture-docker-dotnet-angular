import { CommonModule } from '@angular/common';
import { Component, OnInit, inject, signal } from '@angular/core';
import {
    ReactiveFormsModule,
    UntypedFormBuilder,
    UntypedFormGroup,
    Validators,
} from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { LoginService } from './login.service';
import { NotificationService } from '@core/services/notification.service';

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrl: './login.component.scss',
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
        MatCheckboxModule
    ]
})
export class LoginComponent implements OnInit {
    // Services using inject pattern
    private formBuilder = inject(UntypedFormBuilder);
    private route = inject(ActivatedRoute);
    private router = inject(Router);
    private loginService = inject(LoginService);
    private notificationService = inject(NotificationService);

    // State using signals
    model = signal<any>({});
    loading = signal<boolean>(false);
    returnUrl = signal<string>('');
    hidePassword = signal<boolean>(true);

    loginForm!: UntypedFormGroup;

    ngOnInit(): void {
        // reset login status
        this.createForm();

        // get return url from route parameters or default to "/"
        this.returnUrl.set(this.route.snapshot.queryParams[`returnUrl`] || '/');

        // Retrieve the username from localStorage if it exists
        if (typeof window !== 'undefined') {
            const savedUsername = localStorage.getItem('rememberedUsername');
            if (savedUsername) {
                this.loginForm.controls['email'].setValue(savedUsername);
                this.loginForm.controls['rememberMe'].setValue(true);
            }
        }
    }

    createForm(): void {
        this.loginForm = this.formBuilder.group({
            email: ['', [Validators.required, Validators.email]],
            password: ['', Validators.required],
            rememberMe: [false],
        });
    }

    togglePasswordVisibility(): void {
        this.hidePassword.update(value => !value);
    }

    onSubmit(): void {
        this.login(this.loginForm);
    }

    login(loginForm: UntypedFormGroup): void {
        if (loginForm.valid) {
            this.loading.set(true);
            this.loginService
                .login(
                    loginForm.controls['email'].value,
                    loginForm.controls['password'].value
                )
                .subscribe({
                    next: (data) => {
                        this.loading.set(false);
                        if (loginForm.controls['rememberMe'].value) {
                            localStorage.setItem(
                                'rememberedUsername',
                                loginForm.controls['email'].value
                            );
                        } else {
                            localStorage.removeItem('rememberedUsername');
                        }
                        this.router.navigate([this.returnUrl()]);
                    },
                    error: (error) => {
                        this.notificationService.error(error?.message || 'Login failed');
                        this.loading.set(false);
                    }
                });
        } else {
            this.notificationService.error('Please enter valid credentials');
        }
    }
}
