import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { CommonModule } from '@angular/common';
import { ListComponentComponent } from '../list-component/list-component.component';
import { FormsModule } from '@angular/forms';
import { HttpParams } from '@angular/common/http';
import { MatDialog } from '@angular/material/dialog';

@Component({
    selector: 'app-my-lists',
    standalone: true,
    imports: [CommonModule, ListComponentComponent, FormsModule],
    templateUrl: './my-lists.component.html',
    styleUrls: ['./my-lists.component.css']  // Fix typo here
  })
  export class MyListsComponent implements OnInit {
    lists$: Observable<any[]> = of([]);
    searchQuery: string = '';
    isWishlist: boolean = false;
    isReadingList: boolean = false;
    pageNumber: number = 1;
    pageSize: number = 10;
    totalRecords: number = 0;
  
    constructor(private http: HttpClient, private dialog: MatDialog) {}
  
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

          const userId = localStorage.getItem('UserId');
    
        this.http.get<any>(`https://localhost:7267/api/List/user/${userId}`, { params }).subscribe(response => {
          this.totalRecords = response.totalRecords;
          this.lists$ = of(response.lists);
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
