import { Component, inject, OnInit, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
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
import { MatProgressSpinnerModule } from "@angular/material/progress-spinner";

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.scss'],
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatSelectModule,
    MatInputModule,
    MatButtonModule,
    MatTableModule,
    MatIconModule,
    MatProgressSpinnerModule
  ]
})
export class UsersComponent implements OnInit {
  private fb = inject(FormBuilder);
  private userService = inject(UserService);
  private snackBar = inject(MatSnackBar);
  
  displayedColumns = signal<string[]>(['firstName', 'lastName', 'userName', 'email', 'mobile', 'actions']);
  users = signal<User[]>([]);
  selectedUser = signal<User | null>(null);
  loading = signal<boolean>(false);
  
  userForm = this.fb.group({
    id: [''],
    firstName: ['', Validators.required],
    lastName: ['', Validators.required],
    userName: ['', Validators.required],
    email: ['', [Validators.required, Validators.email]],
    mobile: ['', Validators.required]
  });

  ngOnInit(): void {
    this.loadUsers();
  }

  loadUsers(): void {
    this.loading.set(true);
    this.userService.getAll().subscribe({
      next: (users) => {
        console.log('Users loaded:', users);
        this.users.set(users);
        this.loading.set(false);
      },
      error: (err) => {
        console.error('Failed to load users:', err);
        this.snackBar.open('Failed to load users', 'Close', { duration: 3000 });
        this.loading.set(false);
      }
    });
  }

  selectUser(user: User): void {
    this.selectedUser.set(user);
    this.userForm.patchValue({
      id: user.id,
      firstName: user.firstName,
      lastName: user.lastName,
      userName: user.userName,
      email: user.email,
      mobile: user.mobile
    });
  }

  saveUser(): void {
    if (this.userForm.invalid) {
      return;
    }

    const userData = this.userForm.value as User;
    const userId = this.userForm.get('id')?.value;
    
    this.loading.set(true);
    if (userId) {
      userData.id = userId;
      this.userService.update(userData).subscribe({
        next: () => {
          this.snackBar.open('User updated successfully', 'Close', { duration: 3000 });
          this.loadUsers();
          this.resetForm();
        },
        error: (err) => {
          console.error('Failed to update user:', err);
          this.snackBar.open('Failed to update user', 'Close', { duration: 3000 });
          this.loading.set(false);
        }
      });
    } else {
      this.userService.create(userData).subscribe({
        next: () => {
          this.snackBar.open('User created successfully', 'Close', { duration: 3000 });
          this.loadUsers();
          this.resetForm();
        },
        error: (err) => {
          console.error('Failed to create user:', err);
          this.snackBar.open('Failed to create user', 'Close', { duration: 3000 });
          this.loading.set(false);
        }
      });
    }
  }

  deleteUser(user: User): void {
    if (confirm(`Are you sure you want to delete ${user.firstName} ${user.lastName}?`)) {
      this.loading.set(true);
      this.userService.delete(user.id).subscribe({
        next: () => {
          this.snackBar.open('User deleted successfully', 'Close', { duration: 3000 });
          this.loadUsers();
        },
        error: (err) => {
          console.error('Failed to delete user:', err);
          this.snackBar.open('Failed to delete user', 'Close', { duration: 3000 });
          this.loading.set(false);
        }
      });
    }
  }

  resetForm(): void {
    this.selectedUser.set(null);
    this.userForm.reset();
    this.loading.set(false);
  }
}
