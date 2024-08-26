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
    // Add logout logic here
    console.log('Logged out');
    this.router.navigate(['/login']);
    this.isMenuOpen = false;  // Close the menu on logout
  }
}
