import { DecimalPipe, CommonModule } from '@angular/common';
import { Component, ViewChild, OnInit, computed, inject, signal } from '@angular/core';
import { FormControl, FormsModule, ReactiveFormsModule } from '@angular/forms';
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
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { ContactService } from '../contact.service';
import { HasPermissionDirective } from '../../../@core/directives/permission.directive';

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
        MatSnackBarModule,
        HasPermissionDirective
    ],
    templateUrl: './contact-list.component.html',
    styleUrl: './contact-list.component.scss',
    providers: [DecimalPipe, ContactService]
})
export class ContactListComponent implements OnInit {
    private contactService = inject(ContactService);
    private router = inject(Router);

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
                this.contactService.showNotification('Failed to load contacts', true);
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
                    this.contactService.showNotification('Contact deleted successfully');
                    this.loadContacts();
                },
                error: (error) => {
                    this.contactService.showNotification('Error deleting contact', true);
                }
            });
        }
    }
}
