import { CommonModule } from '@angular/common';
import { Component, OnInit, inject, signal } from '@angular/core';
import {
    ReactiveFormsModule,
    UntypedFormBuilder,
    UntypedFormGroup,
    Validators,
} from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTabsModule } from '@angular/material/tabs';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { Router } from '@angular/router';
import { UserService } from '@core/services/user.service';
import { ValidationService } from '@core/services/validation.service';
import { AuthStateService } from '@core/services/auth-state.service';
import { NotificationService } from '@core/services/notification.service';
import { User } from "../../../@core/models/user.interface";

@Component({
    selector: 'app-profile',
    standalone: true,
    imports: [
        ReactiveFormsModule,
        CommonModule,
        MatFormFieldModule,
        MatInputModule,
        MatButtonModule,
        MatIconModule,
        MatTabsModule,
        MatProgressSpinnerModule
    ],
    templateUrl: './profile.component.html',
    styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {
    active = 0;
    user = signal<any>(null);
    userData = signal<any>(null);
    profileForm!: UntypedFormGroup;
    passwordForm!: UntypedFormGroup;
    loading = false;
    isEditMode = signal<boolean>(false);

    private fb = inject(UntypedFormBuilder);
    private router = inject(Router);
    private userService = inject(UserService);
    private validationService = inject(ValidationService);
    private notificationService = inject(NotificationService);
    private authState = inject(AuthStateService);

    constructor() {
        this.createProfileForm();
        this.createPasswordForm();
    }

    toggleEditMode(): void {
        if (this.isEditMode()) {
            // Reset form to original values when canceling
            this.profileForm.patchValue(this.userData());
        }
        this.isEditMode.update(current => !current);
    }

    onSubmit(): void {
        if (this.profileForm.valid) {
            this.loading = true;
            this.updateProfile();
        } else {
            // Mark all fields as touched to trigger validation messages
            this.profileForm.markAllAsTouched();
            this.notificationService.error('Please fix form errors before submitting');
        }
    }

    changePassword(): void {
        if (this.passwordForm.valid) {
            this.loading = true;
            this.updatePassword();
        } else {
            // Mark all fields as touched to trigger validation messages
            this.passwordForm.markAllAsTouched();
            this.notificationService.error('Please fix form errors before submitting');
        }
    }

    createProfileForm(): void {
        this.profileForm = this.fb.group({
            id: [''],
            firstName: ['', Validators.required],
            lastName: ['', Validators.required],
            userName: ['', Validators.required],
            email: ['', Validators.required],
            mobile: ['', [Validators.required, Validators.minLength(10)]],
        });
    }

    createPasswordForm(): void {
        this.passwordForm = this.fb.group(
            {
                currentPassword: ['', Validators.required],
                password: ['', [Validators.required, Validators.minLength(6)]],
                confirmPassword: ['', Validators.required],
            },
            {
                validator: this.validationService.MustMatch(
                    'password',
                    'confirmPassword'
                ),
            }
        );
    }

    resetProfileForm() {
        this.profileForm.reset();
        this.profileForm.patchValue(this.userService.getCurrentUser());
    }

    updateProfile() {
        // Only send the fields that should be updated
        const profileUpdate : User = {
            id: this.profileForm.value.id,
            firstName: this.profileForm.value.firstName,
            lastName: this.profileForm.value.lastName,
            userName: this.profileForm.value.userName,
            email: this.profileForm.value.email,
            mobile: this.profileForm.value.mobile
        };

        this.userService.update(profileUpdate).subscribe({
            next: (data) => {
                // Preserve role permissions from the original user data
                const updatedUserData = {
                    ...this.userData(),
                    ...data
                };

                console.log('updatedUserData', updatedUserData);
                const currentUser = JSON.parse(localStorage.getItem('currentUser') || '{}');
                updatedUserData.rolePermissions = currentUser.rolePermissions;
                updatedUserData.token = currentUser.token;
                console.log('Again updates', updatedUserData);
                // Update both signals
                this.user.set(updatedUserData);
                this.userData.set(updatedUserData);
                this.authState.updateUser(updatedUserData); // Update shared state

                this.notificationService.success('Profile updated successfully');
                this.loading = false;
                this.isEditMode.set(false);
            },
            error: (error) => {
                this.loading = false;
                const errorMessage = error?.error?.message || 'Error updating profile';
                this.notificationService.error(errorMessage);
            }
        });
    }

    resetPasswordForm() {
        this.passwordForm.reset();
    }

    updatePassword() {
        const passwordData = {
            currentPassword: this.passwordForm.get('currentPassword')?.value,
            newPassword: this.passwordForm.get('password')?.value
        };

        this.userService
            .changePassword(passwordData)
            .subscribe({
                next: (data) => {
                    this.notificationService.success('Password updated successfully');
                    this.resetPasswordForm();
                    this.loading = false;
                    this.router.navigate(['/login']);
                },
                error: (error) => {
                    this.loading = false;
                    const errorMessage = error?.error?.message || 'Error updating password';
                    this.notificationService.error(errorMessage);
                }
            });
    }

    ngOnInit(): void {
        this.userService.getCurrentUser().subscribe({
            next: (userData) => {
                this.user.set(userData);
                this.userData.set(userData);
                this.profileForm.patchValue(userData);
            },
            error: (error) => {
                this.notificationService.error('Failed to load user profile');
                this.loading = false;
            }
        });
    }
}
