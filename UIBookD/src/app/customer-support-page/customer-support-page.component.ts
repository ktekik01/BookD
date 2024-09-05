import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { NgModel } from '@angular/forms';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-customer-support-page',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './customer-support-page.component.html',
  styleUrl: './customer-support-page.component.css'
})
export class CustomerSupportPageComponent {

    private requestUrl = 'https://localhost:7267/api/CustomerSupport/CreateRequest';

    // Properties to bind to the input fields
    public title: string = '';
    public content: string = '';

    constructor(private http: HttpClient) { }
    
    ngOnInit() {
        const userId = localStorage.getItem('UserId'); // Make sure this is retrieved correctly
    }

    addRequest() {
        const userId = localStorage.getItem('UserId'); // Make sure this is retrieved correctly
    
        if (!userId || !this.title || !this.content) {
            console.error('All fields are required.');
            return; // Exit if fields are missing
        }
    
        const request = {
            userId: userId,
            title: this.title,
            content: this.content
        };
    
        this.http.post(this.requestUrl, request).subscribe(
            (response) => {
                console.log('Request sent successfully:', response);
                this.title = '';     // Reset the title field after submission
                this.content = '';   // Reset the content field after submission
            },
            (error) => {
                console.error('Error occurred:', error);
                console.log(error.error.errors); // Print validation errors, if any
            }
        );
    }
    
}
