import { CommonModule } from '@angular/common';
import { Component, OnInit, computed, inject, signal } from '@angular/core';
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
import { MatIconModule } from '@angular/material/icon';
import { MatSelectModule } from '@angular/material/select';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { ValidationService } from '@core/services/validation.service';

import { errorTailorImports } from "@core/components/validation";
import { ContactService } from "@features/contact/contact.service";
import { NotificationService } from '@core/services/notification.service';

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
        MatProgressSpinnerModule,
        MatIconModule,
        MatSelectModule
    ],
    templateUrl: './contact-form.component.html',
    styleUrl: './contact-form.component.scss',
    providers: [ContactService]
})
export class ContactFormComponent implements OnInit {
    private fb = inject(UntypedFormBuilder);
    private router = inject(Router);
    private validationService = inject(ValidationService);
    private contactService = inject(ContactService);
    private activatedRoute = inject(ActivatedRoute);
    private notificationService = inject(NotificationService);

    contactForm!: UntypedFormGroup;
    loading = signal<boolean>(false);
    isEditMode = signal<boolean>(false);
    contact = signal<any>(null);

    formValid = computed(() => this.contactForm?.valid ?? false);

    onSubmit(): void {
        if (this.formValid()) {
            const contact = this.contactForm.value;
            this.loading.set(true);

            if (this.isEditMode()) {
                this.contactService.update(contact).subscribe({
                    next: () => {
                        this.notificationService.success('Contact updated successfully');
                        this.router.navigate(['/contacts']);
                    },
                    error: (error) => {
                        this.loading.set(false);
                        this.notificationService.error('Error updating contact');
                    }
                });
            } else {
                this.contactService.create(contact).subscribe({
                    next: () => {
                        this.notificationService.success('Contact created successfully');
                        this.router.navigate(['/contacts']);
                    },
                    error: (error) => {
                        this.loading.set(false);
                        this.notificationService.error('Error creating contact');
                    }
                });
            }
        }
    }

    createForm(): void {
        this.contactForm = this.fb.group({
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
        this.loading.set(true);
        this.contactService.create(contact).subscribe({
            next: (data) => {
                this.notificationService.success('Contact created successfully');
                this.router.navigate(['/contacts']);
            },
            error: (error) => {
                this.loading.set(false);
                this.notificationService.error('Error creating contact');
            }
        });
    }

    update(contact: any): void {
        this.loading.set(true);
        this.contactService.update(contact).subscribe({
            next: (data) => {
                this.notificationService.success('Contact updated successfully');
                this.router.navigate(['/contacts']);
            },
            error: (error) => {
                this.loading.set(false);
                this.notificationService.error('Error updating contact');
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
            this.contact.set(contactDetails);
            this.isEditMode.set(true);
            this.contactForm.patchValue(contactDetails);
            this.contactForm.controls.dateOfBirth.setValue(this.formatDate(contactDetails.dateOfBirth));
        }
    }
    private formatDate(jsonDate: string): string {
      const date = new Date(jsonDate);
      return date.toISOString().split('T')[0]; // yyyy-MM-dd format
    }
}
