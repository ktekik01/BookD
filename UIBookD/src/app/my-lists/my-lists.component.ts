import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { CommonModule } from '@angular/common';
import { ListComponentComponent } from '../list-component/list-component.component';

@Component({
    selector: 'app-my-lists',
    standalone: true,
    imports: [CommonModule, ListComponentComponent],
    templateUrl: './my-lists.component.html',
    styleUrls: ['./my-lists.component.css']  // Fix typo here
  })
  export class MyListsComponent implements OnInit {
    lists$: Observable<any[]> = of([]); // Provide an initial value
  
    constructor(private http: HttpClient) {}
  
    ngOnInit(): void {
      this.getLists();
    }
  
    getLists(): void {
      const userId = localStorage.getItem('UserId'); // Retrieve userId from localStorage
      console.log('test: ',userId);
      this.lists$ = this.http.get<any[]>(`https://localhost:7267/api/List/user/${userId}`);
    }
  }
