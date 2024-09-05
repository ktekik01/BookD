import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { CommonModule } from '@angular/common';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { ErrorPopupComponent } from '../error-popup/error-popup.component';
import { NgModule } from '@angular/core';
import { Dialog } from '@angular/cdk/dialog';
import { DialogModule } from '@angular/cdk/dialog';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css'],
  standalone: true,
  imports: [CommonModule, ErrorPopupComponent, MatDialogModule]
})
export class DashboardComponent implements OnInit {
  isMenuOpen = false;
  showUnauthorizedMessage = false;

  constructor(private router: Router, private route: ActivatedRoute, public dialog: MatDialog) {}

  ngOnInit() {
    // Check if the 'unauthorized' query parameter exists
    console.log(localStorage.getItem('UserId'));
    console.log(localStorage.getItem('UserType'));
    this.route.queryParams.subscribe(params => {
      if (params['unauthorized']) {
        this.showUnauthorizedMessage = true;
        // Optionally, hide the message after a few seconds
        setTimeout(() => this.showUnauthorizedMessage = false, 5000);
      }
    });
  }

  toggleMenu() {
    this.isMenuOpen = !this.isMenuOpen;
  }

  navigateTo(page: string) {
    if (page === 'admin-panel') {

        // print the user type

      // Check if the user is an admin
      const userType = localStorage.getItem('UserType');
      console.log(userType);

      const userId = localStorage.getItem('UserId');
      console.log("User ID adasd:", userId);
      console.log("User type adasd:", userType);


      if (userType !== 'Admin') {
        this.showErrorPopup('You are not authorized to access the Admin Panel.');
        return;
      }
    }
    this.router.navigate([`/${page}`]);
    this.isMenuOpen = false;  // Close the menu on navigation
  }
  

  showErrorPopup(message: string) {
    this.dialog.open(ErrorPopupComponent, {
      data: { message: message }
    });
  }

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
    this.router.navigate(['/home']);
    this.isMenuOpen = false;  // Close the menu on logout
  }

}
