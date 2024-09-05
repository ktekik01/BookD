import { Component, Input } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-user-component',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './user-component.component.html',
  styleUrl: './user-component.component.css'
})
export class UserComponentComponent {
    @Input() user: any; // Accepts user data as input
  
    constructor(private http: HttpClient) { }

    // user id
    senderId: string | null = null;
    receiverId: string = '';
    private chatApiUrl = 'https://localhost:7267/api/Chatting/StartChat';


    // function to start chat with user
    toChat() {
        
        this.senderId = localStorage.getItem('UserId');
        this.receiverId = this.user.id;
        // check if user tries to chat with himself
        if (this.senderId === this.receiverId) {
            alert('You cannot chat with yourself');
            return;
        }

        const chatRequest = {
            senderId: this.senderId,
            receiverId: this.receiverId
        };

        console.log(chatRequest);

        this.http.post(this.chatApiUrl, chatRequest).subscribe((response: any) => {
            if (response) {
                alert('Chat started');
            }
        });


    }
}
