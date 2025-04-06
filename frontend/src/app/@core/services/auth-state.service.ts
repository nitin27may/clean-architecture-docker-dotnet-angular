import { Injectable, signal, inject, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { User } from '@core/models/user.interface';

@Injectable({
  providedIn: 'root'
})
export class AuthStateService {
  private platformId = inject(PLATFORM_ID);

  // Create a user state signal with null initial value
  private userState = signal<User | null>(null);

  // Update the user data in the signal and localStorage
  updateUser(user: User) {
    this.userState.set(user);

    if (isPlatformBrowser(this.platformId)) {
      try {
        const currentUser = JSON.parse(localStorage.getItem('currentUser') || '{}');
        const updatedUser = { ...currentUser, ...user };
        localStorage.setItem('currentUser', JSON.stringify(updatedUser));
      } catch (error) {
        console.error('Error updating user in localStorage:', error);
      }
    }
  }

  // Get the current user signal
  getCurrentUser() {
    return this.userState;
  }

  // Initialize user data from localStorage
  initializeFromStorage() {
    if (isPlatformBrowser(this.platformId)) {
      try {
        const savedUser = localStorage.getItem('currentUser');
        if (savedUser) {
          this.userState.set(JSON.parse(savedUser));
        }
      } catch (error) {
        console.error('Error reading user from localStorage:', error);
      }
    }
  }
}
