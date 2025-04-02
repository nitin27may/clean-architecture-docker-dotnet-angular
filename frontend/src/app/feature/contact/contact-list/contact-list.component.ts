import { DecimalPipe, CommonModule } from '@angular/common';
import { Component, OnInit, ViewChild } from '@angular/core';
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
import { ContactService } from '../contact.service';
import { ToastrService } from 'ngx-toastr';

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
        MatSortModule
    ],
    templateUrl: './contact-list.component.html',
    styleUrl: './contact-list.component.scss',
    providers: [DecimalPipe, ContactService]
})
export class ContactListComponent implements OnInit {
    dataSource: MatTableDataSource<any>;
    displayedColumns: string[] = ['name', 'email', 'phone', 'city', 'actions'];
    @ViewChild(MatSort) sort: MatSort;

    constructor(
        private contactService: ContactService,
        private router: Router,
        private toastr: ToastrService
    ) {
        this.dataSource = new MatTableDataSource();
    }

    applyFilter(event: Event) {
        const filterValue = (event.target as HTMLInputElement).value;
        this.dataSource.filter = filterValue.trim().toLowerCase();
    }

    getAll(): void {
        this.contactService.getAll().subscribe({
            next: (data) => {
                data.sort((a, b) => new Date(b.create_date).getTime() - new Date(a.create_date).getTime());
                this.dataSource.data = data;
                this.dataSource.sort = this.sort;
            },
            error: (error) => {
                this.toastr.error('Failed to load contacts');
                console.error(error);
            }
        });
    }

    deleteContact(contact: any): void {
        if (confirm('Are you sure you want to delete this contact?')) {
            this.contactService.delete(contact.id).subscribe({
                next: () => {
                    this.toastr.success('Contact deleted successfully');
                    this.getAll();
                },
                error: (error) => {
                    this.toastr.error('Failed to delete contact');
                    console.error(error);
                }
            });
        }
    }

    ngOnInit(): void {
        this.getAll();
    }
}
