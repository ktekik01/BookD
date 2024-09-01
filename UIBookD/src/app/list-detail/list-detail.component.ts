import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { map, switchMap } from 'rxjs/operators';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatDialogModule } from '@angular/material/dialog';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';

interface ListDetail {
  name: string;
  description: string;
  ownerName: string;
  type: string;
  ownerId: string;  // Ensure ownerId is part of ListDetail
  bookNames: string[];
}

@Component({
  selector: 'app-list-detail',
  standalone: true,
  templateUrl: './list-detail.component.html',
  styleUrls: ['./list-detail.component.css'],
  imports: [
    CommonModule,
    FormsModule,
    MatInputModule,
    MatButtonModule,
    MatDialogModule
  ]
})
export class ListDetailComponent implements OnInit {

  listDetail$!: Observable<ListDetail | null>;
  newBookName: string = '';  // Model for the new book name
  currentUserId: string = ''; // ID of the currently logged-in user
  private apiUrl = 'https://localhost:7267/api/List/contents';
  private addBookApiUrl = 'https://localhost:7267/api/List/addBook';
  private deleteBookApiUrl = 'https://localhost:7267/api/List/deleteBook';

  constructor(
    private route: ActivatedRoute,
    private http: HttpClient
  ) { }

  ngOnInit(): void {
    // Get current user ID from localStorage
    const currentUserId = localStorage.getItem('UserId');
    if (currentUserId) {
      this.currentUserId = currentUserId;
    }
    else {
        console.error('No user ID found in localStorage.');
        }

    this.listDetail$ = this.route.paramMap.pipe(
      map(params => params.get('id')),
      switchMap(id => {
        if (id) {
          return this.http.get<ListDetail>(`${this.apiUrl}/${id}`);
        } else {
          console.error('No ID found in the route');
          return of(null);
        }
      })
    );
  }

  addBookToList() {
    const id = this.route.snapshot.paramMap.get('id');
    const payload = {
      listId: id,
      bookName: this.newBookName
    };

    if (id) {
      this.http.post(this.addBookApiUrl, payload, {
        headers: { 'Content-Type': 'application/json' }
      })
      .subscribe({
        next: (response) => {
          console.log('Book added successfully:', response);
          this.refreshListDetail();
        },
        error: (error) => {
          console.error('Error adding book:', error);
        }
      });
    } else {
      console.error('No list ID found in route.');
    }
  }

  deleteBookFromList(bookName: string): void {
    if (confirm(`Are you sure you want to remove "${bookName}" from the list?`)) {
      const id = this.route.snapshot.paramMap.get('id');
      if (id) {
        const payload = {
          listId: id,
          bookName: bookName
        };

        this.http.delete(this.deleteBookApiUrl, { body: payload })
          .subscribe({
            next: (response: any) => {
              console.log('Book deleted successfully:', response.message);
              this.refreshListDetail();
            },
            error: (error) => {
              console.error('Error deleting book:', error.error);
            }
          });
      } else {
        console.error('No list ID provided.');
      }
    }
  }

  isListOwner(listDetail: ListDetail): boolean {
    return this.currentUserId === listDetail.ownerId;
  }

  refreshListDetail(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.listDetail$ = this.http.get<ListDetail>(`${this.apiUrl}/${id}`);
    }
  }
}
