import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar'; // Import MatSnackBar for displaying notifications

@Injectable({
  providedIn: 'root',
})

export class NotificationService {

  constructor(private snackBar: MatSnackBar) { }

  showSuccess(message: string, action: string = 'Close'): void {
    this.snackBar.open(message, action, {
      duration: 3000, // Duration in milliseconds
      panelClass: ['success-snackbar']
    });
  }

  showError(message: string, action: string = 'Close'): void {
    this.snackBar.open(message, action, {
      duration: 3000,
      panelClass: ['error-snackbar']
    });
  }
}
