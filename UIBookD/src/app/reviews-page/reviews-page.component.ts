import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { Observable, of } from 'rxjs';
import { AddReviewModalComponent } from '../add-review-modal/add-review-modal.component';
import { CommonModule } from '@angular/common';
import { ReviewComponent } from '../review-component/review-component.component';

@Component({
  selector: 'app-reviews-page',
  standalone: true,
  imports: [
    CommonModule,
    ReviewComponent,
    MatDialogModule
  ],
  templateUrl: './reviews-page.component.html',
  styleUrls: ['./reviews-page.component.css']
})

export class ReviewsPageComponent implements OnInit {

  reviews$: Observable<any[]> = of([]); // Provide an initial value

  private apiUrl = 'https://localhost:7267/api/Review'; // Replace with your API URL

  constructor(private http: HttpClient, private dialog: MatDialog) { }

  ngOnInit(): void {
    this.getReviews();
  }

  getReviews(): void {
    this.reviews$ = this.http.get<any[]>(this.apiUrl);
  }

  openAddReviewModal(): void {
    const dialogRef = this.dialog.open(AddReviewModalComponent);
  
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        const userId = localStorage.getItem('UserId');
  
        const reviewData = {
          ...result,
          userId: userId
        };
  
        console.log('Review Data:', reviewData); // Log the payload
  
        this.http.post(this.apiUrl, reviewData).subscribe(
          () => {
            this.getReviews();
          },
          (error) => {
            console.error('Error occurred:', error.error); // Log the error details
          }
        );
      }
    });
  }
  

}
