import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ChatService {
  private hubConnection: HubConnection;

  private apiUrl = 'https://localhost:7267/api/Chatting'; // Adjust as necessary

  private messagesSubject = new BehaviorSubject<any[]>([]);
  public messages$ = this.messagesSubject.asObservable();

  constructor(private http: HttpClient) {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl('https://localhost:7267/chathub') // URL to your SignalR hub
      .build();


      this.startConnection();

      /*
      this.hubConnection.start()
      .then(() => {
        console.log('SignalR connection established');
        this.onMessageReceived((senderId, message) => {
          console.log('Message received:', senderId, message);
          // Update the UI with the new message
            this.updateMessages(senderId, message);
        });
      })
      .catch(err => console.error('Error starting SignalR connection:', err));

      this.hubConnection.on('ReceiveMessage', (senderId: string, content: string) => {
        console.log(`Message received from ${senderId}: ${content}`);
        this.updateMessages(senderId, content);
        }); */
  }

  private async startConnection(): Promise<void> {
    try {
      await this.hubConnection.start();
      console.log('SignalR connection established');
      this.setUpListeners();
    } catch (err) {
      console.error('Error while starting SignalR connection:', err);
    }
  }
  
  private setUpListeners(): void {
    this.hubConnection.on('ReceiveMessage', (senderId: string, content: string) => {
      console.log('Message received via SignalR:', senderId, content);
      this.updateMessages(senderId, content);
    });
  }

  private updateMessages(senderId: string, content: string) {
    const currentMessages = this.messagesSubject.getValue();
    this.messagesSubject.next([...currentMessages, { senderId, content }]);
  }

  /*
  loadMessages(chatId: string) {
    this.http.get<any[]>(`${this.apiUrl}/chat/${chatId}/messages`).subscribe(
      messages => {
        this.messagesSubject.next(messages);
      },
      error => {
        console.error('Error loading messages:', error);
      }
    );
  } */

    loadMessages(chatId: string) {
        this.getMessages(chatId).subscribe(
          messages => {
            this.messagesSubject.next(messages);
          },
          error => {
            console.error('Error loading messages:', error);
          }
        );
      }


  // method to fetch chats of the parameterized user.

  getChats(userId: string): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/GetChats/${userId}`);
  }

  // method to fetch messages of a chat.
  getMessages(chatId: string): Observable<any[]> {
    return this.http.get<any[]>(`https://localhost:7267/api/Chatting/chat/${chatId}/messages`);
}


    // Public method to handle received messages
    public onMessageReceived(callback: (senderId: string, message: string) => void): void {
        this.hubConnection.on('ReceiveMessage', (senderId: string, message: string) => {
          console.log('Message received via SignalR:', senderId, message);
          callback(senderId, message);
        });
      }
      

    public  sendMessage(receiverId: string, senderId: string, messageContent: string) {
        console.log("Sending message via SignalR:", receiverId, senderId, messageContent);
        this.hubConnection.send('SendMessage', senderId, receiverId, messageContent)
          .catch(err => console.error('Error sending message: ', err));
      }
}
