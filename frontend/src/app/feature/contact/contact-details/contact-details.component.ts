import { Component, computed, inject, signal } from '@angular/core';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { ContactService } from "@features/contact/contact.service";
import { CommonModule } from "@angular/common";
import { ReactiveFormsModule } from "@angular/forms";
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { HasPermissionDirective } from '@core/directives/permission.directive';

@Component({
    selector: 'app-contact-details',
    standalone: true,
    imports: [
        RouterModule,
        CommonModule,
        ReactiveFormsModule,
        MatIconModule,
        MatButtonModule,
        MatTooltipModule,
        MatSnackBarModule,
        HasPermissionDirective
    ],
    templateUrl: './contact-details.component.html',
    styleUrl: './contact-details.component.scss',
    providers: [ContactService]
})
export class ContactDetailsComponent {
    private activatedRoute = inject(ActivatedRoute);
    private router = inject(Router);
    private contactService = inject(ContactService);

    contact = signal<any>(null);
    loading = signal<boolean>(false);

    // Computed properties for template
    fullName = computed(() => {
        const c = this.contact();
        return c ? `${c.firstName} ${c.lastName}` : 'Loading...';
    });

    deleteContact(): void {
        if (confirm('Are you sure you want to delete this contact?')) {
            if (!this.contact()) return;

            this.loading.set(true);
            this.contactService.delete(this.contact()!.id).subscribe({
                next: () => {
                    this.contactService.showNotification('Contact deleted successfully');
                    this.router.navigate(['/contacts']);
                },
                error: (error) => {
                    this.loading.set(false);
                    this.contactService.showNotification('Failed to delete contact', true);
                }
            });
        }
    }

    ngOnInit(): void {
        const contactData = this.activatedRoute.snapshot.data['contactDetails'];
        if (contactData) {
            this.contact.set(contactData);
        }
    }
}
