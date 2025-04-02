import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
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
import { ToastrService } from 'ngx-toastr';
import { LoginService } from './login.service';

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
    ],
    providers: [LoginService]
})
export class LoginComponent implements OnInit {
    model: any = {};
    loading = false;
    returnUrl: string;
    loginForm: UntypedFormGroup;
    hidePassword = true;
    constructor(
        private formBuilder: UntypedFormBuilder,
        private route: ActivatedRoute,
        private router: Router,
        private loginService: LoginService,
        private toastrService: ToastrService
    ) {}

    ngOnInit(): void {
        // reset login status
        this.createForm();

        // get return url from route parameters or default to "/"
        this.returnUrl = this.route.snapshot.queryParams[`returnUrl`] || '/' ;

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
        this.hidePassword = !this.hidePassword;
    }
    
    onSubmit(): void {
        this.login(this.loginForm);
    }

    login(loginForm: UntypedFormGroup): void {
        if (loginForm.valid) {
            this.loading = true;
            this.loginService
                .login(
                    loginForm.controls.email.value,
                    loginForm.controls.password.value
                )
                .subscribe({
                    next: (data) => {
                        this.loading = false;
                        if (loginForm.controls.rememberMe.value) {
                            localStorage.setItem(
                                'rememberedUsername',
                                loginForm.controls.email.value
                            );
                        } else {
                            localStorage.removeItem('rememberedUsername');
                        }
                        this.router.navigate([this.returnUrl]);
                    },
                    error: (error) => {
                        this.toastrService.error(error?.message || 'Login failed');
                        this.loading = false;
                    }
                });
        } else {
            this.toastrService.error('Please enter valid credentials');
        }
    }
}
