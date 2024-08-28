import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common'; // Import CommonModule

@Component({
  selector: 'app-my-reviews',
  standalone: true,
  templateUrl: './my-reviews.component.html',
  styleUrls: ['./my-reviews.component.css'],
  imports: [CommonModule] // Add CommonModule here
})
export class MyReviewsComponent implements OnInit {
  reviews: any[] = [];  // Array to store the reviews

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.getMyReviews();
  }

  getMyReviews(): void {
    const userId = localStorage.getItem('UserId'); // Retrieve userId from localStorage
    
    if (userId) {
      // Log userId for debugging
      console.log('Retrieved userId:', userId);

      this.http.get<any[]>(`https://localhost:7267/api/Review/user/${userId}`)
        .subscribe({
          next: (data) => {
            console.log('API call successful:', data); // Debug line
            this.reviews = data;
          },
          error: (err) => {
            console.error('Error fetching user reviews:', err); // Log detailed error
          }
        });
    } else {
      console.error('User ID is not found in local storage');
    }
  }
}
