import { CommonModule } from '@angular/common';
import { Observable, of } from 'rxjs';
import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BookComponentComponent } from '../book-component/book-component.component';

@Component({
  selector: 'app-books-page',
  standalone: true,
  imports: [CommonModule, BookComponentComponent], // Import CommonModule here
  templateUrl: './books-page.component.html',
  styleUrl: './books-page.component.css'
})
export class BooksPageComponent implements OnInit {
    books$: Observable<any[]> = of([]); // Provide an initial value

    private apiUrl = 'https://localhost:7267/api/Book'; // Replace with your API URL

    constructor(private http: HttpClient) { }

    ngOnInit(): void {
        this.getBooks();
      }
    
      getBooks(): void {
        this.books$ = this.http.get<any[]>(this.apiUrl);
      }

}
