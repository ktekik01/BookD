import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ReviewComponent } from '../review-component/review-component.component';
import { Observable, of } from 'rxjs';

@Component({
  selector: 'app-my-reviews',
  standalone: true,
  templateUrl: './my-reviews.component.html',
  styleUrls: ['./my-reviews.component.css'],
  imports: [CommonModule, ReviewComponent, FormsModule]
})
export class MyReviewsComponent implements OnInit {

    reviews$: Observable<any[]> = of([]);
    totalReviews: number = 0;
    page: number = 1;
    pageSize: number = 10;
    title: string = '';
    book: string = '';
    sortBy: string = 'reviewDate';
    sortDescending: boolean = false;

    private apiUrl = 'https://localhost:7267/api/Review/user';

    constructor(private http: HttpClient) {}

    ngOnInit(): void {
        this.getMyReviews();
    }

    getMyReviews(): void {
        const userId = localStorage.getItem('UserId');
        
        if (userId) {
            let params = new HttpParams()
                .set('title', this.title)
                .set('book', this.book)
                .set('page', this.page.toString())
                .set('pageSize', this.pageSize.toString())
                .set('sortBy', this.sortBy)
                .set('SortDescending', this.sortDescending.toString());

            this.http.get<any>(`${this.apiUrl}/${userId}`, { params }).subscribe(response => {
                this.reviews$ = of(response.reviews);
                this.totalReviews = response.totalReviews;
            });
        }
    }

    onPageChange(newPage: number): void {
        this.page = newPage;
        this.getMyReviews();
    }

    onSortChange(sortBy: string): void {
        this.sortBy = sortBy;
        this.sortDescending = sortBy.endsWith('Desc');
        this.getMyReviews();
    }
}
