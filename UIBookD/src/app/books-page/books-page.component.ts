import { CommonModule } from '@angular/common';
import { Observable, of } from 'rxjs';
import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BookComponentComponent } from '../book-component/book-component.component';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { AddBookModalComponent } from '../add-book-modal/add-book-modal.component';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { ReactiveFormsModule } from '@angular/forms';





@Component({
  selector: 'app-books-page',
  standalone: true,
  imports: [CommonModule, BookComponentComponent, CommonModule,
    MatDialogModule,     CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatDatepickerModule,
    MatNativeDateModule], // Import CommonModule here
  templateUrl: './books-page.component.html',
  styleUrl: './books-page.component.css'
})
export class BooksPageComponent implements OnInit {
    books$: Observable<any[]> = of([]); // Provide an initial value

    private apiUrl = 'https://localhost:7267/api/Book'; // Replace with your API URL

    constructor(private http: HttpClient, private dialog: MatDialog) { }

    ngOnInit(): void {
        this.getBooks();
      }
    
      getBooks(): void {
        this.books$ = this.http.get<any[]>(this.apiUrl);
      }

    openAddBookModal(): void {
        const dialogRef = this.dialog.open(AddBookModalComponent);
      
        dialogRef.afterClosed().subscribe(result => {
          if (result) {
            const userId = localStorage.getItem('UserId');

            if(!userId) {
                console.error('User ID not found');
                return;
            }
      
            console.log('Book Data:', result); // Log the payload
      
            this.http.post(this.apiUrl, result).subscribe(
              () => {
                this.getBooks();
              },
              (error) => {
                console.error('Error occurred:', error.error); // Log the error details
              }
            );
          }
        });
      }

}
