import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { MatDialog } from '@angular/material/dialog';
import { AddListModalComponent } from '../add-list-modal/add-list-modal.component';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ListComponentComponent } from '../list-component/list-component.component';

@Component({
  selector: 'app-lists-page',
  templateUrl: './lists-page.component.html',
  styleUrls: ['./lists-page.component.css'],
  standalone: true,
  imports: [FormsModule, CommonModule, ListComponentComponent],
})
export class ListsPageComponent implements OnInit {
    lists$: Observable<any[]> = of([]);
    searchQuery: string = '';
    isWishlist: boolean = false;
    isReadingList: boolean = false;
    pageNumber: number = 1;
    pageSize: number = 10;
    totalRecords: number = 0;
  private apiUrl = 'https://localhost:7267/api/List'; // Replace with your API URL

  constructor(private http: HttpClient, private dialog: MatDialog) { }

  ngOnInit(): void {
    this.getLists();
  }

  getLists(): void {
    let listType = '';
    if (this.isWishlist) listType = 'wishlist';
    if (this.isReadingList) listType = 'reading list';

    let params = new HttpParams()
      .set('searchQuery', this.searchQuery)
      .set('listType', listType)
      .set('pageNumber', this.pageNumber)
      .set('pageSize', this.pageSize);

    this.http.get<any>(this.apiUrl, { params }).subscribe(response => {
      this.totalRecords = response.totalRecords;
      this.lists$ = of(response.lists);
      console.log(response);
    });
  }

  openAddListModal(): void {
    const dialogRef = this.dialog.open(AddListModalComponent);

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        const userId = localStorage.getItem('UserId');
        const listData = {
          ...result,
          userId: userId
        };

        if (listData.type === 'wishlist') {
          this.http.post(`${this.apiUrl}/wishlist`, listData).subscribe(() => {
            this.getLists();
          });
        } else {
          this.http.post(`${this.apiUrl}/readinglist`, listData).subscribe(() => {
            this.getLists();
          });
        }
      }
    });
  }

  goToNextPage(): void {
    this.pageNumber++;
    this.getLists();
  }

  goToPreviousPage(): void {
    if (this.pageNumber > 1) {
      this.pageNumber--;
      this.getLists();
    }
  }
}
