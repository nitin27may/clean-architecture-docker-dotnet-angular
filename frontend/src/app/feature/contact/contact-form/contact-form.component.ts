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
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { ValidationService } from '../../../@core/services/validation.service';
import { ContactService } from '../contact.service';
import { errorTailorImports } from "../../../@core/components/validation";

@Component({
    selector: 'app-contact-form',
    standalone: true,
    imports: [
        ReactiveFormsModule, 
        RouterModule, 
        CommonModule, 
        errorTailorImports, 
        MatFormFieldModule,
        MatInputModule,
        MatButtonModule,
        MatDatepickerModule, 
        MatNativeDateModule,
        MatProgressSpinnerModule
    ],
    templateUrl: './contact-form.component.html',
    styleUrl: './contact-form.component.scss',
    providers: [ContactService]
})
export class ContactFormComponent implements OnInit {
    contactForm: UntypedFormGroup;
    loading = false;
    isEditMode = false;

    constructor(
        private formBuilder: UntypedFormBuilder,
        private router: Router,
        private validationService: ValidationService,
        private contactService: ContactService,
        private activatedRoute: ActivatedRoute,
        private toastrService: ToastrService
    ) {}

    onSubmit(): void {
        if (this.contactForm.invalid) {
            return;
        }
        this.submit();
    }

    createForm(): void {
        this.contactForm = this.formBuilder.group({
            id: ['', []],
            firstName: [
                '',
                [
                    Validators.required,
                    Validators.minLength(2),
                    Validators.maxLength(35),
                ],
            ],
            lastName: [
                '',
                [
                    Validators.required,
                    Validators.minLength(2),
                    Validators.maxLength(35),
                ],
            ],
            dateOfBirth:[],
            email: [
                '',
                [Validators.required, this.validationService.emailValidator],
            ],
            countryCode: ['', [Validators.required]],
            mobile: ['', [Validators.required]],
            city: ['', [Validators.required]],
            postalCode: ['', [Validators.required]],
        });
    }

    reset(): void {
        const contact = this.contactForm.value;
        if (contact.id) {
            this.getContactDetails();
        } else {
            this.contactForm.reset();
        }
    }
    submit(): void {
        const contact = this.contactForm.value;
        if (contact.id) {
            this.update(contact);
        } else {
            delete contact.id;
            this.save(contact);
        }
    }

    save(contact: any): void {
        this.loading = true;
        this.contactService.create(contact).subscribe({
            next: (data) => {
                this.toastrService.success('Contact created successfully', 'Success');
                this.router.navigate(['/contacts']);
            },
            error: (error) => {
                this.loading = false;
                this.toastrService.error(error.message || 'Error creating contact');
            }
        });
    }

    update(contact: any): void {
        this.loading = true;
        this.contactService.update(contact).subscribe({
            next: (data) => {
                this.toastrService.success('Contact updated successfully', 'Success');
                this.router.navigate(['/contacts']);
            },
            error: (error) => {
                this.loading = false;
                this.toastrService.error(error.message || 'Error updating contact');
            }
        });
    }

    ngOnInit(): void {
        this.createForm();
        this.getContactDetails();
    }

    private getContactDetails() {
        const contactDetails = this.activatedRoute.snapshot.data.contactDetails;
        if (contactDetails) {
            this.isEditMode = true;
            this.contactForm.patchValue(contactDetails);
            this.contactForm.controls.dateOfBirth.setValue(this.formatDate(contactDetails.dateOfBirth));
        }
    }
    private formatDate(jsonDate: string): string {
      const date = new Date(jsonDate);
      return date.toISOString().split('T')[0]; // yyyy-MM-dd format
    }
}
