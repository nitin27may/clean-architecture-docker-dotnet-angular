import { Component, inject, OnInit, signal } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { UserService } from '@core/services/user.service';
import { User } from '@core/models/user.interface';
import { CommonModule } from "@angular/common";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatSelectModule } from "@angular/material/select";
import { MatInputModule } from "@angular/material/input";
import { MatButtonModule } from "@angular/material/button";
import { MatTableModule } from "@angular/material/table";
import { MatIconModule } from "@angular/material/icon";

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.scss'],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatSelectModule,
    MatInputModule,
    MatButtonModule,
    MatTableModule,
    MatIconModule
  ]

})
export class UsersComponent implements OnInit {
  private fb = inject(FormBuilder);
  private userService = inject(UserService);
  private snackBar = inject(MatSnackBar);
  displayedColumns: string[] = ['firstName', 'lastName', 'userName', 'email', 'mobile', 'actions'];
  users = signal<User[]>([]);
  selectedUser = signal<User | null>(null);
  userForm: FormGroup;

  constructor() {
    this.userForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      userName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      mobile: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    this.loadUsers();
  }

  loadUsers(): void {
    this.userService.getAll().subscribe({
      next: (users) => this.users.set(users),
      error: (err) => this.snackBar.open('Failed to load users', 'Close', { duration: 3000 })
    });
  }

  selectUser(user: User): void {
    this.selectedUser.set(user);
    this.userForm.patchValue(user);
  }

  saveUser(): void {
    if (this.userForm.invalid) {
      return;
    }

    const userData = this.userForm.value as User;
    if (this.selectedUser()) {
      this.userService.update(userData).subscribe({
        next: () => {
          this.snackBar.open('User updated successfully', 'Close', { duration: 3000 });
          this.loadUsers();
          this.resetForm();
        },
        error: (err) => this.snackBar.open('Failed to update user', 'Close', { duration: 3000 })
      });
    } else {
      this.userService.create(userData).subscribe({
        next: () => {
          this.snackBar.open('User created successfully', 'Close', { duration: 3000 });
          this.loadUsers();
          this.resetForm();
        },
        error: (err) => this.snackBar.open('Failed to create user', 'Close', { duration: 3000 })
      });
    }
  }

  deleteUser(user: User): void {
    this.userService.delete(user.id).subscribe({
      next: () => {
        this.snackBar.open('User deleted successfully', 'Close', { duration: 3000 });
        this.loadUsers();
      },
      error: (err) => this.snackBar.open('Failed to delete user', 'Close', { duration: 3000 })
    });
  }

  resetForm(): void {
    this.selectedUser.set(null);
    this.userForm.reset();
  }
}
