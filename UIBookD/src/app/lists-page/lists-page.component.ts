import { CommonModule } from '@angular/common';
import { Observable, of } from 'rxjs';
import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ListComponentComponent } from '../list-component/list-component.component';

@Component({
  selector: 'app-lists-page',
  standalone: true,
  imports: [CommonModule, ListComponentComponent],
  templateUrl: './lists-page.component.html', 
  styleUrl: './lists-page.component.css'
})
export class ListsPageComponent implements OnInit {
    lists$: Observable<any[]> = of([]); // Provide an initial value

    private apiUrl = 'https://localhost:7267/api/List'; 

    constructor(private http: HttpClient) { }

    ngOnInit(): void {
        this.getLists();
      }
    
      getLists(): void {
        this.lists$ = this.http.get<any[]>(this.apiUrl);
      }

}
