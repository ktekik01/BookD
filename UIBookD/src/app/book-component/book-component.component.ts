import { Component, Input } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-book-component',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './book-component.component.html',
  styleUrls: ['./book-component.component.css']
})
export class BookComponentComponent {
  
  @Input() book: any; // Accepts book data as input
  raterId: string | null = '12345'; // Replace with real user ID
  selectedRating: number = 0; // The user's selected rating
  errorMessage: string = ''; // Declare the errorMessage property

  constructor(private http: HttpClient) { }


  ngOnInit(): void {
    this.getUserRating();
  }



  // Fetch the user's rating for this book
  getUserRating() {
    const bookId = this.book.id;
    
    this.http.get(`https://localhost:7267/api/Book/rate/${bookId}/${localStorage.getItem('UserId')}`)
      .subscribe({
        next: (response: any) => {
          this.selectedRating = response.rating;
          console.log('User rating fetched:', this.selectedRating);
        },
        error: (error) => {
          this.errorMessage = error.error || 'Error fetching user rating';
          console.error('Error fetching user rating:', this.errorMessage);
        }
      });
  }
  
  rateBook() {
    const ratingData = {
      userId: localStorage.getItem('UserId'),
      rating: this.selectedRating
    };

    const bookId = this.book.id;

    console.log(ratingData.userId, ratingData.rating);
    this.http.post(`https://localhost:7267/api/Book/rate/${bookId}`, ratingData)
      .subscribe({
        next: (response: any) => {
          console.log('Rating successful', response);
          // Optionally, update book average rating on the frontend
          this.book.averageRating = response.averageRating;
        },
      });
  }
}
