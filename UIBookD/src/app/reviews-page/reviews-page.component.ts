import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { MatDialog } from '@angular/material/dialog';
import { AddReviewModalComponent } from '../add-review-modal/add-review-modal.component';
import { FormsModule } from '@angular/forms';
import { NgModule } from '@angular/core';
import { ReviewComponent } from '../review-component/review-component.component';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { NotificationService } from '../notification.service';
import { MatSnackBarModule } from '@angular/material/snack-bar';

@Component({
  selector: 'app-reviews-page',
  templateUrl: './reviews-page.component.html',
  styleUrls: ['./reviews-page.component.css'],
  standalone: true,
  imports: [FormsModule, ReviewComponent, CommonModule, MatSnackBarModule],
})
export class ReviewsPageComponent implements OnInit {

    reviews$: Observable<any[]> = of([]);
    totalReviews: number = 0;
    page: number = 1;
    pageSize: number = 10;
    title: string = '';
    user: string = '';
    book: string = '';
    sortBy: string = 'reviewDate';
    sortDescending: boolean = false;
    id: string = ''; // Ensure id is part of ReviewDetail

    private apiUrl = 'https://localhost:7267/api/Review'; // Replace with your API URL

    constructor(private http: HttpClient, private dialog: MatDialog, private router: Router, private notificationService: NotificationService) { }

    ngOnInit(): void {
        console.log('Total Reviews:', this.totalReviews);
        console.log('Reviews:', this.reviews$);

        this.getReviews();
    }

    getReviews(): void {
        let params = new HttpParams()
            .set('id', this.id)
            .set('title', this.title)
            .set('user', this.user)
            .set('book', this.book)
            .set('page', this.page.toString())
            .set('pageSize', this.pageSize.toString())
            .set('sortBy', this.sortBy)
            .set('SortDescending', this.sortDescending.toString());

        this.http.get<any>(this.apiUrl, { params }).subscribe(response => {
            this.reviews$ = of(response.reviews);
            this.totalReviews = response.totalReviews;
            this.notificationService.showSuccess('Reviews loaded successfully!');
        }, error => {
            this.notificationService.showError('Failed to load reviews.');
        
            
            
        });
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
                        this.notificationService.showSuccess('Review added successfully!');
                        this.getReviews();
                    },
                    (error) => {
                        console.error('Error occurred:', error.error); // Log the error details
                    }
                );
            }
        });
    }

    onPageChange(newPage: number): void {
        this.page = newPage;
        this.getReviews();
    }

    onSortChange(sortBy: string): void {
        this.sortBy = sortBy;
        this.sortDescending = sortBy.endsWith('Desc'); // Determine sorting direction
        this.getReviews();
    }

}
