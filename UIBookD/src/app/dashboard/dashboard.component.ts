import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css'],
  standalone: true,
  imports: [CommonModule]
})
export class DashboardComponent {
  isMenuOpen = false;

  constructor(private router: Router) {}

  toggleMenu() {
    this.isMenuOpen = !this.isMenuOpen;
  }

  navigateTo(page: string) {
    this.router.navigate([`/${page}`]);
    this.isMenuOpen = false;  // Close the menu on navigation
  }

  // navigate to review page
    navigateToReview() {
        this.router.navigate(['/review']);
        this.isMenuOpen = false;  // Close the menu on navigation
    }

    logout() {
        // Clear localStorage or any other storage where authentication data is kept
        localStorage.removeItem('UserId');  // Example of removing a specific item
        localStorage.clear();  // Clear all items if necessary
        
        // Optionally, clear any session storage or cookies if used
        // sessionStorage.clear();
        // document.cookie = 'your_cookie_name=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;';
      
        // Log out the user and redirect to the login page
        console.log('Logged out');
        this.router.navigate(['/login']);
        this.isMenuOpen = false;  // Close the menu on logout
      }
}
