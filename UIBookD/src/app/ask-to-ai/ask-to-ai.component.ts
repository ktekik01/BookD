import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http'; // Import HttpClient to send requests
import { CommonModule } from '@angular/common'; // Import CommonModule for Angular features
import { NgModule } from '@angular/core'; // Import NgModule to create a standalone component
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-ask-to-ai',
  standalone: true,
  imports: [CommonModule, FormsModule], // Import CommonModule
  templateUrl: './ask-to-ai.component.html',
  styleUrls: ['./ask-to-ai.component.css']
})
export class AskToAIComponent {
  userQuestion: string = ''; // To store the user's input
  aiResponse: string = ''; // To store the AI's response
  isLoading: boolean = false; // To indicate when the AI is processing the request
  private apiUrl = 'https://localhost:7267/api/AskToAi/Ask'; // Replace with actual API URL

  constructor(private http: HttpClient) {}

  // Method to send the question to the AI
  askAI() {
    if (!this.userQuestion) {
      return; // Don't send if the input is empty
    }

    this.isLoading = true; // Show loading indicator
    this.aiResponse = ''; // Clear previous response

    const payload = {
      userId: localStorage.getItem('UserId'), // Get the user ID from local storage
      questionText: this.userQuestion,
      answerId: null
    };

    // Send the question to the AI and get the response
    this.http.post<{ answer: string }>(this.apiUrl, payload).subscribe(
      (response) => {
        this.aiResponse = response.answer;
        this.isLoading = false; // Hide loading indicator
      },
      (error) => {
        console.error('Error communicating with AI:', error);
        this.aiResponse = 'Sorry, there was an error processing your question.';
        this.isLoading = false; // Hide loading indicator
      }
    );
  }
}
