import { Injectable, inject } from '@angular/core';
import { MatSnackBar, MatSnackBarConfig } from '@angular/material/snack-bar';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  private snackBar = inject(MatSnackBar);
  private defaultConfig: MatSnackBarConfig = {
    duration: 3000,
    horizontalPosition: 'right',
    verticalPosition: 'top'
  };

  success(message: string): void {
    this.snackBar.open(message, 'Close', {
      ...this.defaultConfig,
      panelClass: ['success-snackbar']
    });
  }

  error(message: string): void {
    this.snackBar.open(message, 'Close', {
      ...this.defaultConfig,
      duration: 5000,
      panelClass: ['error-snackbar']
    });
  }

  warning(message: string): void {
    this.snackBar.open(message, 'Close', {
      ...this.defaultConfig,
      duration: 4000,
      panelClass: ['warning-snackbar']
    });
  }

  info(message: string): void {
    this.snackBar.open(message, 'Close', {
      ...this.defaultConfig,
      panelClass: ['info-snackbar']
    });
  }
}
