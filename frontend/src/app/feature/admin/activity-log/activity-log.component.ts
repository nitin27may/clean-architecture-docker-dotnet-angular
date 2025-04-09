import { Component, ViewChild, AfterViewInit, signal, computed, effect, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator, MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { UserService } from "@core/services/user.service";
import { MatTableModule } from '@angular/material/table';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { DatePipe, NgFor, NgIf } from '@angular/common';

// Define the ActivityLog interface
interface ActivityLog {
  username: string;
  email: string;
  activity: string;
  endpoint: string;
  timestamp: Date;
}

@Component({
  selector: 'app-activity-log',
  templateUrl: './activity-log.component.html',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    MatFormFieldModule,
    MatInputModule,
    DatePipe,
    NgIf
  ]
})
export class ActivityLogComponent implements AfterViewInit, OnInit {
  private fb = inject(FormBuilder);
  private activityLogService = inject(UserService);

  // Signals
  activityLogs = signal<ActivityLog[]>([]);
  totalRecords = signal<number>(0);
  loading = signal<boolean>(false);
  error = signal<string | null>(null);
  pageSize = signal<number>(5);
  pageIndex = signal<number>(0);

  // Material table related properties
  displayedColumns = ['username', 'email', 'activity', 'endpoint', 'timestamp'];
  dataSource = signal(new MatTableDataSource<ActivityLog>([]));

  // Form for API filtering
  filterForm = this.fb.group({
    username: [''],
    email: ['']
  });

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor() {
    // Effect to update the dataSource when activityLogs changes
    effect(() => {
      const currentLogs = this.activityLogs();
      this.totalRecords.set(currentLogs.length);

      // Manually handle pagination since datasource pagination is not working
      const start = this.pageIndex() * this.pageSize();
      const end = start + this.pageSize();
      const paginatedData = currentLogs.slice(start, end);

      const dataSourceValue = new MatTableDataSource<ActivityLog>(paginatedData);

      if (this.sort) {
        dataSourceValue.sort = this.sort;
      }

      this.dataSource.set(dataSourceValue);
    });
  }

  ngOnInit() {
    // Initialize by loading all logs
    this.loadActivityLogs();
  }

  ngAfterViewInit() {
    // Ensure sort is applied after view initialization
    setTimeout(() => {
      if (this.sort) {
        const dataSourceValue = this.dataSource();
        dataSourceValue.sort = this.sort;
        this.dataSource.set(dataSourceValue);
      }
    });
  }

  // Handle pagination events
  handlePageEvent(event: PageEvent) {
    this.pageSize.set(event.pageSize);
    this.pageIndex.set(event.pageIndex);
  }

  loadActivityLogs() {
    this.loading.set(true);
    this.error.set(null);

    this.activityLogService.getActivityLogs().subscribe({
      next: (logs) => {
        this.activityLogs.set(logs);
        this.pageIndex.set(0); // Reset to first page
        this.loading.set(false);
      },
      error: (err) => {
        console.error('Error loading activity logs', err);
        this.error.set('Failed to load activity logs. Please try again.');
        this.loading.set(false);
      }
    });
  }

  onFilter() {
    const usernameValue = this.filterForm.get('username')?.value;
    const emailValue = this.filterForm.get('email')?.value;

    this.loading.set(true);
    this.error.set(null);

    this.activityLogService.getActivityLogs(usernameValue, emailValue).subscribe({
      next: (logs) => {
        this.activityLogs.set(logs);
        this.pageIndex.set(0); // Reset to first page
        this.loading.set(false);
      },
      error: (err) => {
        console.error('Error filtering activity logs', err);
        this.error.set('Failed to filter activity logs. Please try again.');
        this.loading.set(false);
      }
    });
  }

  // Client-side filtering for the Material table
  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    const filteredLogs = this.activityLogs().filter(log =>
      Object.values(log).some(val =>
        val && val.toString().toLowerCase().includes(filterValue.toLowerCase())
      )
    );

    // Update filtered records
    const start = this.pageIndex() * this.pageSize();
    const end = start + this.pageSize();
    const paginatedData = filteredLogs.slice(start, end);

    const dataSourceValue = new MatTableDataSource<ActivityLog>(paginatedData);
    this.totalRecords.set(filteredLogs.length);
    this.pageIndex.set(0); // Reset to first page

    if (this.sort) {
      dataSourceValue.sort = this.sort;
    }

    this.dataSource.set(dataSourceValue);
  }
}
