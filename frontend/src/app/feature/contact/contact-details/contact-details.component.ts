import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { ContactService } from "../contact.service";

import { CommonModule } from "@angular/common";
import { ReactiveFormsModule } from "@angular/forms";
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { ToastrService } from 'ngx-toastr';

@Component({
    selector: 'app-contact-details',
    standalone: true,
    imports: [RouterModule, CommonModule, ReactiveFormsModule, MatIconModule, MatButtonModule],
    templateUrl: './contact-details.component.html',
    styleUrl: './contact-details.component.scss',
    providers: [ContactService]
})
export class ContactDetailsComponent implements OnInit {
    contact: any;
    constructor(
        private activatedRoute: ActivatedRoute,
        private router: Router,
        private contactService: ContactService,
        private toastrService: ToastrService
    ) {}

    edit(): void {
        this.router.navigate(['/contacts/edit/' + this.contact._id]);
    }
    
    deleteContact(): void {
        if (confirm('Are you sure you want to delete this contact?')) {
            this.contactService.delete(this.contact._id).subscribe({
                next: () => {
                    this.toastrService.success('Contact deleted successfully');
                    this.router.navigate(['/contacts']);
                },
                error: (error) => {
                    this.toastrService.error('Failed to delete contact');
                    console.error(error);
                }
            });
        }
    }
    
    ngOnInit(): void {
        this.contact = this.activatedRoute.snapshot.data.contactDetails;
    }
}
