import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map, switchMap } from 'rxjs/operators';
import { CommonModule } from '@angular/common';
import { of } from 'rxjs';

interface ListDetail {
  name: string;
  description: string;
  ownerName: string;
  type: string;
  bookNames: string[];
}

@Component({
  selector: 'app-list-detail',
  standalone: true,
  templateUrl: './list-detail.component.html',
  styleUrls: ['./list-detail.component.css'],
  imports: [CommonModule]
})
export class ListDetailComponent implements OnInit {

  listDetail$!: Observable<ListDetail | null>; // Using an observable

  private apiUrl = 'https://localhost:7267/api/List/contents'; 

  constructor(
    private route: ActivatedRoute,
    private http: HttpClient
  ) { }

  ngOnInit(): void {
    this.listDetail$ = this.route.paramMap.pipe(
      map(params => params.get('id')),
      switchMap(id => {
        if (id) {
          return this.http.get<ListDetail>(`${this.apiUrl}/${id}`);
        } else {
          console.error('No ID found in the route');
          return of(null); // Return an observable with null if there's no ID
        }
      })
    );
  }
}
