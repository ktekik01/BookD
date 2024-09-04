import { Component, Input } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { catchError, of, tap } from 'rxjs';
import { Router } from '@angular/router';

interface UpvoteRequestModel {
  reviewId: string;
  userId: string | null;
}

@Component({
  selector: 'app-review-component',
  standalone: true,
  templateUrl: './review-component.component.html',
  styleUrls: ['./review-component.component.css'],
})
export class ReviewComponent {
  @Input() review: any; // Accepts review data as input

  private apiUrl = 'https://localhost:7267/api/Review'; // Base API URL
  private upvoteApiUrl = `${this.apiUrl}/upvote`;
  private downvoteApiUrl = `${this.apiUrl}/downvote`;

  upvoteCount: number = 0;
  downvoteCount: number = 0;
  hasUpvoted: boolean = false;
  hasDownvoted: boolean = false;

  constructor(private http: HttpClient, private router: Router) {}

  ngOnInit() {
    this.initializeCounts();
  }

  // Initialize upvote and downvote counts and states from review data
  private initializeCounts() {
    if (this.review) {
      this.upvoteCount = this.review.upvotes ? this.review.upvotes.length : 0;
      this.downvoteCount = this.review.downvotes ? this.review.downvotes.length : 0;
      const userId = localStorage.getItem('UserId');
      this.hasUpvoted = this.review.upvotes?.includes(userId);
      this.hasDownvoted = this.review.downvotes?.includes(userId);
    }
  }

  // Function to handle upvote
  upvote() {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    const userId = localStorage.getItem('UserId');
    const body: UpvoteRequestModel = { reviewId: this.review.id, userId: userId };

    this.http.post<string>(this.upvoteApiUrl, body, { headers, responseType: 'text' as 'json' }).pipe(
      tap(response => {
        console.log('Upvote successful', response);

        if (response.includes("added")) {
          this.upvoteCount++;
          if (this.hasDownvoted) {
            this.downvoteCount--;
            this.hasDownvoted = false; // User switched from downvote to upvote
          }
          this.hasUpvoted = true;
        } else if (response.includes("removed")) {
          this.upvoteCount--;
          this.hasUpvoted = false;
        }
      }),
      catchError(error => {
        console.error('Upvote failed', error);
        return of('Error occurred');
      })
    ).subscribe();
  }

  // Function to handle downvote
  downvote() {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    const userId = localStorage.getItem('UserId');
    const body = { reviewId: this.review.id, userId: userId };

    this.http.post<string>(this.downvoteApiUrl, body, { headers, responseType: 'text' as 'json' }).pipe(
      tap(response => {
        console.log('Downvote successful', response);

        if (response.includes("added")) {
          this.downvoteCount++;
          if (this.hasUpvoted) {
            this.upvoteCount--;
            this.hasUpvoted = false; // User switched from upvote to downvote
          }
          this.hasDownvoted = true;
        } else if (response.includes("removed")) {
          this.downvoteCount--;
          this.hasDownvoted = false;
        }
      }),
      catchError(error => {
        console.error('Downvote failed', error);
        return of('Error occurred');
      })
    ).subscribe();
  }

  // Function to comment on the review
  comment(commentText: string) {
    const body = { reviewId: this.review.id, text: commentText };

    this.http.post<string>(`${this.apiUrl}/Comment`, body).pipe(
      tap(response => {
        console.log('Comment added', response);
        // Handle the comment addition logic if needed
      }),
      catchError(error => {
        console.error('Comment failed', error);
        return of('Error occurred'); // Handle the error
      })
    ).subscribe();
  }


  // function to handle onClick event. the function will navigate to the review-detail page. get the apiUrl and append the review id to it
    onClick(): void {
        this.router.navigate(['/review-detail', this.review.id]);
        console.log('Review ID:', this.review.id); // Log the review ID
    }
}
