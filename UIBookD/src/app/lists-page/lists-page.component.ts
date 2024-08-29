import { CommonModule } from '@angular/common';
import { Observable, of } from 'rxjs';
import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ListComponentComponent } from '../list-component/list-component.component';
import { AddListModalComponent } from '../add-list-modal/add-list-modal.component';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';

@Component({
  selector: 'app-lists-page',
  standalone: true,
  imports: [CommonModule, ListComponentComponent],
  templateUrl: './lists-page.component.html', 
  styleUrl: './lists-page.component.css'
})
export class ListsPageComponent implements OnInit {
    lists$: Observable<any[]> = of([]); // Provide an initial value

    private apiUrl = 'https://localhost:7267/api/List'; // Replace with your API URL
    private apiUrlforwishlist = 'https://localhost:7267/api/List/wishlist';
    private apiUrlforreading = 'https://localhost:7267/api/List/readinglist';

    constructor(private http: HttpClient, private dialog: MatDialog) { }

    ngOnInit(): void {
        this.getLists();
      }
    
      getLists(): void {
        this.lists$ = this.http.get<any[]>(this.apiUrl);
      }


      openAddListModal(): void {const dialogRef = this.dialog.open(AddListModalComponent);
  
        dialogRef.afterClosed().subscribe(result => {
          if (result) {
            const userId = localStorage.getItem('UserId');
      
            const listData = {
              ...result,
              userId: userId
            };
      
            console.log('Review Data:', listData); // Log the payload
      
            if(listData.type === 'wishlist') {
                this.http.post(this.apiUrlforwishlist, listData).subscribe(() => {this.getLists();}, (error) => {console.error('Error occurred:', error.error);});

            }
            else{
                this.http.post(this.apiUrlforreading, listData).subscribe(() => {this.getLists();}, (error) => {console.error('Error occurred:', error.error);});
            }
          }
        });
    }
}
