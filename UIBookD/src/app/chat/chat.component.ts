import { ChangeDetectorRef, Component, OnInit, OnChanges, OnDestroy, SimpleChanges } from '@angular/core';
import { ChatService } from '../services/chat.services';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Input } from '@angular/core';
import { provideHttpClient } from '@angular/common/http';
import {HttpClient} from '@angular/common/http';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css'],
  standalone: true,
  imports: [ CommonModule, FormsModule]
})
export class ChatComponent implements OnChanges {
    @Input() chat: any; // Define the type if you have a specific model
  messages: any = [];
  userId = ''; // this user ID
  otherUserId = ''; // the other user ID
  message = '';
    chatId: string | null = null;

  constructor(private chatService: ChatService, private cd: ChangeDetectorRef, private route: ActivatedRoute) { }

  
  /*
  ngOnInit() {

    if (this.chat) {
        const chatId = this.chat.id;
        console.log("Chat ID:", chatId);
  
        // Load messages using the chatId from the input chat object
        this.chatService.loadMessages(chatId);
  
        // Set up the receiverId and senderId
        this.userId = localStorage.getItem('UserId') ?? '';
  
        if (this.chat.usersList[0] === this.userId) {
          this.otherUserId = this.chat.usersList[1];
        } else {
          this.otherUserId = this.chat.usersList[0];
        }
      } else {
        console.log("No chat provided as input");
      }

      this.chatService.messages$.subscribe(messages => {
        this.messages = messages;
        this.cd.detectChanges(); // Trigger change detection to update the UI
      });

            // Handle real-time message updates
        this.chatService.onMessageReceived((senderId: string, message: string) => {
            this.messages.push({ senderId, content: message });
            this.cd.detectChanges(); // Trigger UI update
        });

  } */



        ngOnChanges(changes: SimpleChanges) {
            if (changes['chat'] && this.chat) {
              const chatId = this.chat.id;
              console.log("Chat ID:", chatId);
        
              // Load messages using the chatId from the input chat object
              this.chatService.loadMessages(chatId);
        
              // Set up the receiverId and senderId
              this.userId = localStorage.getItem('UserId') ?? '';
        
              if (this.chat.usersList[0] === this.userId) {
                this.otherUserId = this.chat.usersList[1];
              } else {
                this.otherUserId = this.chat.usersList[0];
              }
        
              // Clear previous messages and subscribe to new messages
              this.messages = [];
              this.chatService.messages$.subscribe(messages => {
                this.messages = messages;
                this.cd.detectChanges(); // Trigger change detection to update the UI
              });
        
              // Handle real-time message updates
              this.chatService.onMessageReceived((senderId: string, message: string) => {
                this.messages.push({ senderId, content: message });
                this.cd.detectChanges(); // Trigger UI update
              });
            }
          }

  sendMessage() {
    console.log("Sending message:", this.otherUserId, this.userId, this.message);
    if (this.otherUserId && this.message) {
      console.log("Sending message:", this.otherUserId, this.userId, this.message);
      this.chatService.sendMessage(this.otherUserId, this.userId, this.message);
      this.message = '';  // Clear the input after sending
    } else {
      console.log("Missing receiverId or message");
    }
  }
  
}
