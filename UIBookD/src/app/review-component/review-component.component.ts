import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-review-component',
  standalone: true,
  imports: [],
  templateUrl: './review-component.component.html',
  styleUrl: './review-component.component.css'
})

export class ReviewComponent {
    @Input() review: any; // Accepts review data as input
  
    constructor() { }
  }
