import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-my-profile',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],  // Import ReactiveFormsModule here
  templateUrl: './my-profile.component.html',
  styleUrls: ['./my-profile.component.css']
})
export class MyProfileComponent implements OnInit {
  private apiUrl = 'https://localhost:7267/api/User';
  userForm: FormGroup;
  userId: string | null = '12345';  // Replace with the actual user ID
  isUpdating: boolean = false;
  isDeleting: boolean = false;
  errorMessage: string = '';

  constructor(
    private http: HttpClient,
    private fb: FormBuilder,
    private router: Router
  ) {
    this.userForm = this.fb.group({
      name: [''],
      surname: [''],
      email: [''],
      password: [''],
      profilePicture: [''],
      biography: [''],
      dateOfBirth: ['']
    });
  }

  ngOnInit(): void {
    this.loadUserProfile();
  }

  loadUserProfile(): void {
    this.userId = localStorage.getItem('UserId');  // Get user ID from local storage
    this.http.get<any>(`${this.apiUrl}/${this.userId}`)
      .subscribe(
        (user: any) => {
          this.userForm.patchValue(user);
        },
        (error) => {
          this.errorMessage = 'Failed to load user profile';
        }
      );
  }

  updateProfile(): void {
    this.isUpdating = true;
    const updatedUser = this.userForm.value;

    this.http.put<any>(`${this.apiUrl}/${this.userId}`, updatedUser)
      .subscribe(
        () => {
          this.isUpdating = false;
          alert('Profile updated successfully');
        },
        (error) => {
          this.isUpdating = false;
          this.errorMessage = 'Failed to update profile';
        }
      );
  }

  deleteAccount(): void {
    this.isDeleting = true;

    console.log(this.userId);
    this.http.delete<void>(`${this.apiUrl}/${this.userId}`)
      .subscribe(
        () => {
          this.isDeleting = false;
          alert('Account deleted successfully');
          this.router.navigate(['/login']);  // Redirect to login or home page
        },
        (error) => {
          this.isDeleting = false;
          this.errorMessage = 'Failed to delete account';
        }
      );
  }
}
