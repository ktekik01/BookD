import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class AuthGuard implements CanActivate {
  constructor(private router: Router) {}

  canActivate(): boolean {
    const userId = localStorage.getItem('UserId');
    const userType = localStorage.getItem('UserType');

    console.log('User ID:', userId);
    console.log('User Type:', userType);



    if (userType === 'Admin') {
      return true; // Allow access for admins
    } else {
      // Redirect to dashboard and pass a query parameter indicating the error
      this.router.navigate(['/dashboard'], { queryParams: { unauthorized: true } });
      return false; // Block access for non-admins
    }
  }
}
