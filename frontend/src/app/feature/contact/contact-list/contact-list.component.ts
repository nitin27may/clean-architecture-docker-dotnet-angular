import { DecimalPipe, CommonModule } from '@angular/common';
import { Component, ViewChild, OnInit, inject, signal, computed, AfterViewInit } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { MatPaginator, MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatTableModule } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatSort, MatSortModule, Sort } from '@angular/material/sort';
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
export class ContactListComponent implements OnInit, AfterViewInit {
    private contactService = inject(ContactService);
    private router = inject(Router);
    private notificationService = inject(NotificationService);

    contacts = signal<any[]>([]);
    filteredContacts = signal<any[]>([]);
    dataSource = signal<MatTableDataSource<any>>(new MatTableDataSource([]));
    displayedColumns = ['name', 'email', 'phone', 'city', 'actions'];
    loading = signal<boolean>(false);

    // Pagination related signals
    pageSize = signal<number>(10);
    currentPage = signal<number>(0);
    pageSizeOptions = [5, 10, 25, 50];
    totalItems = computed(() => this.filteredContacts().length);

    @ViewChild(MatSort) sort!: MatSort;
    @ViewChild(MatPaginator) paginator!: MatPaginator;

    ngOnInit(): void {
        this.loadContacts();
    }

    ngAfterViewInit() {
        // Connect the sort and paginator after the view is initialized
        if (this.dataSource()) {
            this.dataSource().sort = this.sort;
            this.dataSource().paginator = this.paginator;
        }
    }

    loadContacts(): void {
        this.loading.set(true);
        this.contactService.getAll().subscribe({
            next: (data) => {
                const sortedData = data.sort((a, b) =>
                    new Date(b.create_date).getTime() - new Date(a.create_date).getTime()
                );
                this.contacts.set(sortedData);
                this.filteredContacts.set(sortedData);

                const dataSource = new MatTableDataSource(sortedData);
                this.dataSource.set(dataSource);

                // Initialize sort and paginator if they're available
                if (this.sort) {
                    dataSource.sort = this.sort;
                }
                if (this.paginator) {
                    dataSource.paginator = this.paginator;
                }

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

        // Reset to first page when filtering
        if (this.dataSource().paginator) {
            this.dataSource().paginator.firstPage();
        }
    }

    pageChanged(event: PageEvent): void {
        this.pageSize.set(event.pageSize);
        this.currentPage.set(event.pageIndex);

        // Update the data source's pagination parameters
        if (this.dataSource()) {
            // This ensures the MatTableDataSource is aware of the new page size
            this.dataSource().paginator = this.paginator;

            // Force the data source to reflect changes by resetting its data
            // This is important when items per page changes
            const currentData = this.dataSource().data;
            this.dataSource().data = [...currentData];
        }
    }

    sortData(sort: Sort): void {
        if (!sort.active || sort.direction === '') {
            // If sorting is cleared, revert to original data
            this.dataSource().data = this.filteredContacts();
            return;
        }

        this.dataSource().data = [...this.filteredContacts()].sort((a, b) => {
            const isAsc = sort.direction === 'asc';
            switch (sort.active) {
                case 'name':
                    return this.compareNames(a, b, isAsc);
                case 'email':
                    return this.compare(a.email, b.email, isAsc);
                case 'phone':
                    return this.compare(a.mobile, b.mobile, isAsc);
                case 'city':
                    return this.compare(a.city, b.city, isAsc);
                default:
                    return 0;
            }
        });
    }

    compareNames(a: any, b: any, isAsc: boolean): number {
        const nameA = `${a.firstName} ${a.lastName}`.toLowerCase();
        const nameB = `${b.firstName} ${b.lastName}`.toLowerCase();
        return (nameA < nameB ? -1 : 1) * (isAsc ? 1 : -1);
    }

    compare(a: string | number, b: string | number, isAsc: boolean): number {
        return (a < b ? -1 : 1) * (isAsc ? 1 : -1);
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
