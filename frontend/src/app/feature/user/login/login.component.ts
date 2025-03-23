import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import {
    ReactiveFormsModule,
    UntypedFormBuilder,
    UntypedFormGroup,
    Validators,
} from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { LoginService } from './login.service';

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrl: './login.component.scss',
    imports: [RouterModule, ReactiveFormsModule, CommonModule],
    providers: [LoginService]
})
export class LoginComponent implements OnInit {
    model: any = {};
    loading = false;
    returnUrl: string;
    loginForm: UntypedFormGroup;
    isPasswordVisible: boolean = false;
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
        this.returnUrl = this.route.snapshot.queryParams[`returnUrl`] || '/';

        // Retrieve the username from localStorage if it exists
        if (typeof window !== 'undefined') {
            const savedUsername = localStorage.getItem('rememberedUsername');
            if (savedUsername) {
                this.loginForm.controls['userName'].setValue(savedUsername);
                this.loginForm.controls['rememberMe'].setValue(true);
            }
        }
    }
    togglePasswordVisibility(): void {
        this.isPasswordVisible = !this.isPasswordVisible;
    }
    createForm(): void {
        this.loginForm = this.formBuilder.group({
            userName: ['', Validators.required],
            password: ['', Validators.required],
            rememberMe: [false],
        });
    }

    login(loginForm: UntypedFormGroup): void {
        if (loginForm.valid) {
            this.loading = true;
            this.loginService
                .login(
                    loginForm.controls.userName.value,
                    loginForm.controls.password.value
                )
                .subscribe(
                    (data) => {
                        this.loading = false;
                        // Save or remove the username based on the "Remember Me" checkbox
                        if (loginForm.controls.rememberMe.value) {
                            localStorage.setItem(
                                'rememberedUsername',
                                loginForm.controls.userName.value
                            );
                        } else {
                            localStorage.removeItem('rememberedUsername');
                        }
                        this.router.navigate([this.returnUrl]);
                    },
                    (error) => {
                       // this.toastrService.error(error);
                        this.loading = false;
                    }
                );
        } else {
            this.toastrService.error('Please enter valid credentails');
        }
    }
}
