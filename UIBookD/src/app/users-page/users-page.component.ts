import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { CommonModule } from '@angular/common';
import { UserComponentComponent } from '../user-component/user-component.component';

@Component({
  selector: 'app-users-page',
  standalone: true,
  imports: [UserComponentComponent, CommonModule],
  templateUrl: './users-page.component.html',
  styleUrl: './users-page.component.css'
})
export class UsersPageComponent implements OnInit{
    users$: Observable<any[]> = of([]);

    private apiUrl = 'https://localhost:7267/api/User';

    constructor(private http: HttpClient) { }

    ngOnInit(): void {
        this.getUsers();
    }

    getUsers(): void {
        this.users$ = this.http.get<any[]>(this.apiUrl);
    }

}
