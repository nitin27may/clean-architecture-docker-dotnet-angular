import { Injectable } from '@angular/core';
import { UntypedFormGroup } from '@angular/forms';

@Injectable({ providedIn: 'root' })
export class ValidationService {
    // No dependencies to inject, so we can remove the empty constructor

    emailValidator(control: any) {
        // RFC 2822 compliant regex
        if (
            control.value &&
            !control.value.match(
                /[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?/
            )
        ) {
            return { invalidEmailAddress: true };
        } else {
            return null;
        }
    }

    mobileValidator(control: any) {
        // RFC 2822 compliant regex
        if (
            control.value &&
            !control.value.match(
                /^(\+\d{1,2}\s)?\(?\d{3}\)?[\s.-]?\d{3}[\s.-]?\d{4}$/
            )
        ) {
            return { invalidMobile: true };
        } else {
            return null;
        }
    }

    passwordValidator(control: any) {
        // RFC 2822 compliant regex
        if (
            control.value &&
            !control.value.match(
                /^(?=\D*\d)(?=[^a-z]*[a-z])(?=[^A-Z]*[A-Z]).{8,30}$/
            )
        ) {
            return { invalidPassword: true };
        } else {
            return null;
        }
    }

    MustMatch(controlName: string, matchingControlName: string) {
        return (formGroup: UntypedFormGroup) => {
            const control = formGroup.controls[controlName];
            const matchingControl = formGroup.controls[matchingControlName];

            if (!control || !matchingControl) {
                return null; // Return if controls don't exist
            }

            if (!(control.value && matchingControl.value)) {
                // return if any of control does not have value
                return null;
            }

            if (matchingControl.errors && !matchingControl.errors['passwordMustMatch']) {
                // return if another validator has already found an error on the matchingControl
                return null;
            }

            // set error on matchingControl if validation fails
            if (control.value !== matchingControl.value) {
                matchingControl.setErrors({ passwordMustMatch: true });
                return { passwordMustMatch: true };
            } else {
                matchingControl.setErrors(null);
                return null;
            }
        };
    }
}
