import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-book-component',
  standalone: true,
  imports: [],
  templateUrl: './book-component.component.html',
  styleUrl: './book-component.component.css'
})
export class BookComponentComponent {

    @Input() book: any; // Accepts book data as input

  constructor() { }

}
