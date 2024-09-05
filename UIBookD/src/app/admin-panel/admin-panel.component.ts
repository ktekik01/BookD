import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AdminPanelComponentComponent } from '../admin-panel-component/admin-panel-component.component';

@Component({
  selector: 'app-admin-panel',
  standalone: true,
  imports: [CommonModule, AdminPanelComponentComponent],
  templateUrl: './admin-panel.component.html',
  styleUrls: ['./admin-panel.component.css']
})
export class AdminPanelComponent implements OnInit {
  isUnauthorized: boolean = false;
  requests: any[] = []; // Array to hold the requests

  private requestUrl = 'https://localhost:7267/api/CustomerSupport/GetRequests';

  constructor(private http: HttpClient, private router: Router) {}

  ngOnInit() {
    // Check if unauthorized flag is set
    const unauthorizedFlag = localStorage.getItem('isUnauthorized');
    
    if (unauthorizedFlag === 'true') {
      this.isUnauthorized = true;
      localStorage.removeItem('isUnauthorized');
    }

    // Fetch requests from the server
    this.fetchRequests();
  }

  // Fetch requests from the server
  fetchRequests() {
    this.http.get<any[]>(this.requestUrl).subscribe(
      (data) => {
        this.requests = data;
        console.log('Requests:', this.requests);
      },
      (error) => {
        console.error('Error fetching requests:', error);
      }
    );
  }

  // Start chat with the owner of the request
  startChat(request: any) {
    const userId = localStorage.getItem('UserId');
    if (!userId || !request.userId) {
      console.error('User ID not found.');
      return;
    }

    const chatRequest = {
      senderId: userId,
      receiverId: request.userId
    };

    this.http.post('https://localhost:7267/api/Chatting/StartChat', chatRequest).subscribe(
      (response: any) => { 
        console.log('Chat started:', response);
      },
      (error) => {
        console.error('Error starting chat:', error);
      }
    );
  }
}
