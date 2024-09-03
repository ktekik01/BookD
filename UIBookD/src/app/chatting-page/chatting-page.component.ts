import { Component, OnInit } from '@angular/core';
import { ChatService } from '../services/chat.services';
import { CommonModule } from '@angular/common';
import { ChatComponent } from '../chat/chat.component';

@Component({
  selector: 'app-chatting-page',
  standalone: true,
  templateUrl: './chatting-page.component.html',
  styleUrls: ['./chatting-page.component.css'],
  imports: [ CommonModule, ChatComponent ]
})
export class ChattingPageComponent implements OnInit {

  userId = '';
  chats: any[] = []; // List of chats to display
  selectedChat: any | null = null; // Currently selected chat
  receiverId = ''; // receiver ID

  constructor(private chatService: ChatService) { 

  }

  ngOnInit(): void {
    this.userId = localStorage.getItem('UserId') ?? '';
    console.log('asdasdUser ID:', this.userId);
    this.loadChats();
  }

  loadChats(userId: string = this.userId) {
    // Fetch the list of chats from your chat service
    this.chatService.getChats(userId).subscribe(
      (chats) => {
        this.chats = chats;
        // console the chat's Ids.
        console.log('Chats:', chats.map(chat => chat.id));
      },
      (error) => {
        console.error('Error fetching chats', error);
      }
    );
  }

  selectChat(chat: any) {
    // Set the selected chat
    this.selectedChat = chat;

    //console the selected chat's Id
    console.log('Selected chat:', chat.id);
  }
}
