import { Component, Input } from '@angular/core';
import {Router} from '@angular/router';

@Component({
  selector: 'app-list-component',
  standalone: true,
  imports: [],
  templateUrl: './list-component.component.html',
  styleUrl: './list-component.component.css'
})

export class ListComponentComponent {

    @Input() list: any; // Accepts list data as input

    constructor(private router: Router) { }


    onClick(): void {
        this.router.navigate(['/list-detail', this.list.list.id]); // Pass list ID to the new page
      }


}
