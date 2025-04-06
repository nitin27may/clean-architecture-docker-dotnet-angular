import { DecimalPipe, CommonModule } from '@angular/common';
import { Component, ViewChild, OnInit,  inject, signal } from '@angular/core';
import {  FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatTableModule } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ContactService } from '@features/contact/contact.service';
import { HasPermissionDirective } from '@core/directives/permission.directive';
import { NotificationService } from '@core/services/notification.service';

@Component({
    selector: 'app-contact-list',
    standalone: true,
    imports: [
        RouterModule,
        FormsModule,
        ReactiveFormsModule,
        CommonModule,
        MatPaginatorModule,
        MatFormFieldModule,
        MatInputModule,
        MatTableModule,
        MatIconModule,
        MatButtonModule,
        MatTooltipModule,
        MatSortModule,
        HasPermissionDirective
    ],
    templateUrl: './contact-list.component.html',
    styleUrl: './contact-list.component.scss',
    providers: [DecimalPipe, ContactService]
})
export class ContactListComponent implements OnInit {
    private contactService = inject(ContactService);
    private router = inject(Router);
    private notificationService = inject(NotificationService);

    contacts = signal<any[]>([]);
    dataSource = signal<MatTableDataSource<any>>(new MatTableDataSource([]));
    displayedColumns = ['name', 'email', 'phone', 'city', 'actions'];
    loading = signal<boolean>(false);

    @ViewChild(MatSort) sort!: MatSort;

    ngOnInit(): void {
        this.loadContacts();
    }

    loadContacts(): void {
        this.loading.set(true);
        this.contactService.getAll().subscribe({
            next: (data) => {
                const sortedData = data.sort((a, b) =>
                    new Date(b.create_date).getTime() - new Date(a.create_date).getTime()
                );
                this.contacts.set(sortedData);
                this.dataSource.set(new MatTableDataSource(sortedData));
                this.dataSource().sort = this.sort;
                this.loading.set(false);
            },
            error: (error) => {
                this.notificationService.error('Failed to load contacts');
                this.loading.set(false);
            }
        });
    }

    applyFilter(event: Event) {
        const filterValue = (event.target as HTMLInputElement).value;
        this.dataSource().filter = filterValue.trim().toLowerCase();
    }

    deleteContact(contact: any): void {
        if (confirm('Are you sure you want to delete this contact?')) {
            this.contactService.delete(contact.id).subscribe({
                next: () => {
                    this.notificationService.success('Contact deleted successfully');
                    this.loadContacts();
                },
                error: (error) => {
                    this.notificationService.error('Error deleting contact');
                }
            });
        }
    }
}
