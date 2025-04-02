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
import { Router, RouterModule } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { UserService } from '../../../@core/services/user.service';
import { ValidationService } from '../../../@core/services/validation.service';

@Component({
    selector: 'app-register',
    standalone: true,
    imports: [
        RouterModule, 
        ReactiveFormsModule, 
        CommonModule, 
        MatFormFieldModule, 
        MatInputModule, 
        MatIconModule,
        MatProgressSpinnerModule
    ],
    templateUrl: './register.component.html',
    styleUrl: './register.component.scss'
})
export class RegisterComponent implements OnInit {
    loading = false;
    hidePassword = true;
    registerForm: UntypedFormGroup;
    
    constructor(
        private formBuilder: UntypedFormBuilder,
        private router: Router,
        private userService: UserService,
        private toastrService: ToastrService,
        private validationService: ValidationService
    ) {
        this.registerForm = this.createForm();
    }

    onSubmit(): void {
        if (this.registerForm.invalid) {
            return;
        }
        this.register();
    }
    
    register(): void {
        this.loading = true;
        this.userService.create(this.registerForm.value).subscribe({
            next: (data) => {
                this.toastrService.success('Registration successful');
                this.router.navigate(['/login']);
                console.log(data);
            },
            error: (error) => {
                this.loading = false;
                this.toastrService.error(error.message || 'Registration failed');
            }
        });
    }

    createForm(): UntypedFormGroup {
        return this.formBuilder.group(
            {
                firstName: [null, { validators: [Validators.required] }],
                lastName: [null, { validators: [Validators.required] }],
                username: [
                    null,
                    {
                        validators: [
                            Validators.required,
                            this.validationService.emailValidator,
                        ],
                    },
                ],
                password: [
                    null,
                    {
                        validators: [
                            Validators.required,
                            Validators.minLength(6),
                        ],
                    },
                ],
                confirmPassword: [null, { validators: [Validators.required] }],
            },
            {
                validator: this.validationService.MustMatch(
                    'password',
                    'confirmPassword'
                ),
            }
        );
    }
    ngOnInit(): void {}
}
