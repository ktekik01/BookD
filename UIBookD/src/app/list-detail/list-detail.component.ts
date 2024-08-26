import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';  // Import CommonModule

interface ListDetail {
  title: string;
  description: string;
  owner: string;
  type: string;
  // Add other fields as necessary
  
}

@Component({
  selector: 'app-list-detail',
  standalone: true,  // Indicate that this is a standalone component
  templateUrl: './list-detail.component.html',
  styleUrls: ['./list-detail.component.css'],
  imports: [CommonModule]  // Import CommonModule here
})
export class ListDetailComponent implements OnInit {

  listDetail: ListDetail | null = null;  // Initialize listDetail

  private apiUrl = 'https://localhost:7267/api/List/contents'; // Replace with your API URL

  constructor(
    private route: ActivatedRoute,
    private http: HttpClient
  ) { }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');

    if (id) {
      this.http.get<ListDetail>(`${this.apiUrl}/${id}`).subscribe(
        data => {
          console.log('Received data:', data); // Debugging line
          this.listDetail = data;
        },
        error => {
          console.error('Error fetching list details:', error);
        }
      );
    } else {
      console.error('No ID found in the route');
    }
  }
}
