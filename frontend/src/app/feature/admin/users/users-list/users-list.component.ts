import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { UserService } from '@core/services/user.service';

@Component({
  selector: 'app-users-list',
  templateUrl: './users-list.component.html',
  styleUrls: ['./users-list.component.scss']
})
export class UserListComponent implements OnInit {
  filterForm: FormGroup;
  activityLogs: any[] = [];

  constructor(private fb: FormBuilder, private userService: UserService) {}

  ngOnInit(): void {
    this.filterForm = this.fb.group({
      username: [''],
      email: ['']
    });
  }

  onFilter(): void {
    const { username, email } = this.filterForm.value;
    this.userService.getActivityLogs(username, email).subscribe(logs => {
      this.activityLogs = logs;
    });
  }
}
