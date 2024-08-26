import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { CommonModule } from '@angular/common';
import { ReviewComponent } from '../review-component/review-component.component';


@Component({
  selector: 'app-reviews-page',
  standalone: true,
  imports: [CommonModule, ReviewComponent], // Import CommonModule here
  templateUrl: './reviews-page.component.html',
  styleUrls: ['./reviews-page.component.css'] // Fixed styleUrl to styleUrls
})
export class ReviewsPageComponent implements OnInit {
  reviews$: Observable<any[]> = of([]); // Provide an initial value

  private apiUrl = 'https://localhost:7267/api/Review'; // Replace with your API URL

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    this.getReviews();
  }

  getReviews(): void {
    this.reviews$ = this.http.get<any[]>(this.apiUrl);
  }
}
