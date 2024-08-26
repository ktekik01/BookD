// home.component.ts
import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
  standalone: true
})
export class HomeComponent {
  trivia: string = '';

  triviaList: string[] = [
    "Did you know? The world's oldest known library was founded in the 7th century B.C.",
    "Fun fact: 'Bibliosmia' is the enjoyment of the smell of old books.",
    "Did you know? The longest novel ever written is 'In Search of Lost Time' by Marcel Proust.",
  ];

  constructor(private router: Router) {
    this.displayRandomTrivia();
  }

  displayRandomTrivia() {
    const randomIndex = Math.floor(Math.random() * this.triviaList.length);
    this.trivia = this.triviaList[randomIndex];
  }

  navigateToLogin() {
    this.router.navigate(['/login']);
  }

  navigateToRegister() {
    this.router.navigate(['/register']);
  }

    navigateToDashboard() {
        this.router.navigate(['/dashboard']);
    }
}
