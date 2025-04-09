import { Component, inject, OnInit, signal } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { UserService } from '@core/services/user.service';
import { DatePipe, NgFor } from '@angular/common';

@Component({
  selector: 'app-activity-log',
  templateUrl: './activity-log.component.html',
  styleUrls: ['./activity-log.component.scss'],
  standalone: true,
  imports: [
    ReactiveFormsModule,
    DatePipe,
    NgFor
  ]
})
export class ActivityLogComponent implements OnInit {
  private fb = inject(FormBuilder);
  private userService = inject(UserService);

  filterForm!: FormGroup;
  activityLogs = signal<any[]>([]);

  ngOnInit(): void {
    this.filterForm = this.fb.group({
      username: [''],
      email: ['']
    });
  }

  onFilter(): void {
    const { username, email } = this.filterForm.value;
    this.userService.getActivityLogs(username, email).subscribe({
      next: (logs) => {
        this.activityLogs.set(logs);
      }
    });
  }
}
