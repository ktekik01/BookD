import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError, switchMap, tap } from 'rxjs/operators';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule } from '@angular/material/dialog';
import { MatInputModule } from '@angular/material/input';


// upvote and downvote request models

interface UpvoteRequestModel {
  reviewId: string;
  userId: string | null;
}


interface DownvoteRequestModel {
  reviewId: string;
  userId: string | null;
}

interface Comment {
  id: string;
  reviewId: string;
  content: string;
  userId: string;
  commentDate: string;
}


// review detail interface that has bookTitle, userName and review which includes review properties in it



interface ReviewDetail {
    review: {
        id: string;
        title: string;
        reviewText: string;
        userId: string;
        upvoteCount: number;
        downvoteCount: number;
        bookTitle: string;
        reviewDate: string;
        };
    userName: string;
    bookTitle: string;
  }
  
  @Component({
    selector: 'app-review-detail',
    standalone: true,
    imports: [
      CommonModule,
      FormsModule,
      MatInputModule,
      MatButtonModule,
      MatDialogModule
    ],
    templateUrl: './review-detail.component.html',
    styleUrls: ['./review-detail.component.css']
  })
  export class ReviewDetailComponent implements OnInit {
    reviewDetail$!: Observable<ReviewDetail | null>;
    comments$: Observable<Comment[]> = of([]);
    newCommentText: string = '';
    reviewId: string | null = null;
    upvoteCount: number = 0;
    downvoteCount: number = 0;
    hasUpvoted: boolean = false;
    hasDownvoted: boolean = false;
  
    private upvoteApiUrl = 'https://localhost:7267/api/Review/upvote';
    private downvoteApiUrl = 'https://localhost:7267/api/Review/downvote';
    private chatApiUrl = 'https://localhost:7267/api/Chatting/StartChat';
  
    constructor(
      private route: ActivatedRoute,
      private http: HttpClient
    ) { }
  
    ngOnInit(): void {
      this.reviewDetail$ = this.route.paramMap.pipe(
        switchMap(params => {
          const id = params.get('id');
          if (id) {
            this.reviewId = id;
            return this.http.get<any>(`https://localhost:7267/api/Review/${id}`).pipe(
              tap(response => {
                this.reviewDetail$ = of(response);
                this.upvoteCount = response.review.upvotes ? response.review.upvotes.length : 0;
                this.downvoteCount = response.review.downvotes ? response.review.downvotes.length : 0;
                const userId = localStorage.getItem('UserId');
                const reviewUserId = response.review.userId;
                console.log('Review User ID:', reviewUserId);
                this.hasUpvoted = response.review.upvotes?.includes(userId || '');
                this.hasDownvoted = response.review.downvotes?.includes(userId || '');
              }),
              catchError(error => {
                console.error('Error fetching review detail:', error);
                return of(null);
              })
            );
          } else {
            return of(null);
          }
        })
      );

      // Fetch comments
        this.comments$ = this.route.paramMap.pipe(
            switchMap(params => {
            const id = params.get('id');
            if (id) {
                return this.http.get<Comment[]>(`https://localhost:7267/api/Review/comments/${id}`).pipe(
                catchError(error => {
                    console.error('Error fetching comments:', error);
                    return of([]);
                })
                );
            } else {
                return of([]);
            }
            })
        );
    }
  
    addComment(): void {
        if (this.reviewId && this.newCommentText.trim()) {
          const userId = localStorage.getItem('UserId');
          const comment = {
            reviewId: this.reviewId,
            userId: userId,
            content: this.newCommentText.trim() // Ensure this is not empty
          };
      
          console.log('Comment object:', comment); // Log the comment object to verify content
      
          this.http.post<Comment>(`https://localhost:7267/api/Review/comment`, comment, { headers: { 'Content-Type': 'application/json' } }).pipe(
            tap(() => {
              this.newCommentText = '';
              this.refreshComments(); // Refresh comments
            }),
            catchError(error => {
              console.error('Error adding comment:', error);
              if (error.error && error.error.errors) {
                console.error('Validation Errors:', error.error.errors);
              }
              return of(null);
            })
          ).subscribe();
        }
      }
      

      
  
    // refresh comments
    refreshComments(): void {
      this.comments$ = this.route.paramMap.pipe(
        switchMap(params => {
          const id = params.get('id');
          if (id) {
            return this.http.get<Comment[]>(`https://localhost:7267/api/Review/comments/${id}`).pipe(
              catchError(error => {
                console.error('Error fetching comments:', error);
                return of([]);
              })
            );
          } else {
            return of([]);
          }
        })
      );
    }


    upvote(): void {
      if (this.reviewId) {
        const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
        const userId = localStorage.getItem('UserId');
        const body: UpvoteRequestModel = { reviewId: this.reviewId, userId: userId };
  
        this.http.post<string>(this.upvoteApiUrl, body, { headers, responseType: 'text' as 'json' }).pipe(
          tap(response => {
            if (response.includes('Upvote added')) {
              this.upvoteCount++;
              this.hasUpvoted = true;
              if (this.hasDownvoted) {
                this.downvoteCount--;
                this.hasDownvoted = false;
              }
            } else if (response.includes('Upvote removed')) {
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
    }
  
    downvote(): void {
      if (this.reviewId) {
        const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
        const userId = localStorage.getItem('UserId');
        const body: DownvoteRequestModel = { reviewId: this.reviewId, userId: userId };
  
        this.http.post<string>(this.downvoteApiUrl, body, { headers, responseType: 'text' as 'json' }).pipe(
          tap(response => {
            if (response.includes('Downvote added')) {
              this.downvoteCount++;
              this.hasDownvoted = true;
              if (this.hasUpvoted) {
                this.upvoteCount--;
                this.hasUpvoted = false;
              }
            } else if (response.includes('Downvote removed')) {
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
    }


    toChat(): void {
        this.reviewDetail$.subscribe(reviewDetail => {
          if (reviewDetail && reviewDetail.review) {
            const senderId = localStorage.getItem('UserId'); // Get the sender ID from localStorage
            const receiverId = reviewDetail.review.userId; // Assuming the `userId` is part of the `review` object
        
            // Check if the user is trying to chat with themselves
            if (!senderId || senderId === receiverId) {
              console.error('Cannot chat with yourself');
              return; // Stop further execution if the user is trying to chat with themselves
            }
        
            // If the receiverId is available, proceed with the chat request
            if (receiverId) {
              const chatRequest = {
                senderId: senderId,
                receiverId: receiverId
              };
        
              console.log("aslkfdlfjsdşlfkjsdflşj")
              // Make the POST request to start the chat
              this.http.post<string>(this.chatApiUrl, chatRequest).pipe(
                tap(response => {
                  console.log('Chat started successfully:', response);
                  // Add any additional logic like redirecting to the chat page
                }),
                catchError(error => {
                  console.error('Failed to start chat:', error);
                  return of('Error occurred'); // Handle the error
                })
              ).subscribe();
            } else {
              console.error('Receiver ID is not available');
            }
          }
        });
      }
      
    
  }
