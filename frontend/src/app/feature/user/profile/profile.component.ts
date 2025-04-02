import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
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
import { ToastrService } from 'ngx-toastr';
import { UserService } from '../../../@core/services/user.service';
import { ValidationService } from '../../../@core/services/validation.service';

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
    user: any;
    profileForm: UntypedFormGroup;
    passwordForm: UntypedFormGroup;
    loading = false;

    constructor(
        private formBuilder: UntypedFormBuilder,
        private router: Router,
        private userService: UserService,
        private validationService: ValidationService,
        private toastrService: ToastrService
    ) {
        this.createProfileForm();
        this.createPasswordForm();
    }

    onSubmit(): void {
        if (this.profileForm.valid) {
            this.loading = true;
            this.updateProfile();
        }
    }

    changePassword(): void {
        if (this.passwordForm.valid) {
            this.loading = true;
            this.updatePassword();
        }
    }

    createProfileForm(): UntypedFormGroup {
        return (this.profileForm = this.formBuilder.group({
            id: [''],
            firstName: ['', Validators.required],
            lastName: ['', Validators.required],
            userName: ['', Validators.required],
            email: ['', Validators.required],
            mobile: ['', [Validators.required, Validators.minLength(10)]],
        }));
    }
    createPasswordForm() {
        return (this.passwordForm = this.formBuilder.group(
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
        ));
    }

    resetProfileForm() {
        this.profileForm.reset();
        this.profileForm.patchValue(this.userService.getCurrentUser());
    }
    updateProfile() {
        this.userService.update(this.profileForm.value).subscribe(
            (data) => {
                this.toastrService.success('Profile updated successful');
                const user = data;
                user.token = this.user.token;
                localStorage.setItem('currentUser', JSON.stringify(user));
            },
            (error) => {}
        );
    }

    resetPasswordForm() {
        this.passwordForm.reset();
       // this.passwordForm.get('userName').patchValue(this.user.userName);
    }
    updatePassword() {
        this.userService
            .changePassword(
              {
                "currentPassword": this.passwordForm.get('currentPassword').value,
                "newPassword":this.passwordForm.get('password').value
              }


            )
            .subscribe(
                (data) => {
                    this.toastrService.success('Profile updated successful');
                    this.router.navigate(['/login']);
                },
                (error) => {}
            );
    }

    ngOnInit(): void {
        this.userService.getCurrentUser().subscribe((user) => {
           this.user = user;
          console.log(user);
          this.profileForm.patchValue(this.user);
         // this.passwordForm.get('userName').patchValue(this.user.userName);
          });

    }
}
