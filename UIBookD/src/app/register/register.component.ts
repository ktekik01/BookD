import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClient, provideHttpClient } from '@angular/common/http';
import { UploadComponent } from '../upload/upload.component';
import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { NgModule } from '@angular/core';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
  standalone: true,  // Mark the component as standalone
  imports: [ReactiveFormsModule, CommonModule, UploadComponent]
})
export class RegisterComponent {
  registerForm: FormGroup;
  selectedFileUrl: string | null = null; // Store the file URL

  constructor(private fb: FormBuilder, private router: Router, private http: HttpClient) {
    this.registerForm = this.fb.group({
      name: ['', [Validators.required]],
      surname: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      profilePicture: [''],
        Biography: [''],
        DateOfBirth: ['', [Validators.required]],
        

    });
  }

  onRegister(event: Event) {
    event.preventDefault();

    if (this.registerForm.valid) {
        const formData = new FormData();
        
        // Append form values to formData
        const name = this.registerForm.get('name')?.value;
        const surname = this.registerForm.get('surname')?.value;
        const email = this.registerForm.get('email')?.value;
        const password = this.registerForm.get('password')?.value;
        const biography = this.registerForm.get('Biography')?.value;
        const dateOfBirth = this.registerForm.get('DateOfBirth')?.value;

        if (name) formData.append('name', name);
        if (surname) formData.append('surname', surname);
        if (email) formData.append('email', email);
        if (password) formData.append('password', password);
        if (biography) formData.append('biography', biography);
        if (dateOfBirth) formData.append('dateOfBirth', dateOfBirth);

        console.log('Selected File URL:', this.selectedFileUrl); // Log URL
if (this.selectedFileUrl) {
    formData.append('profilePicture', this.selectedFileUrl);
}


        // Log form data
        for (let pair of (formData as any).entries()) {
            console.log(`${pair[0]}: ${pair[1]}`);
        }


        this.http.post('https://localhost:7267/api/User/reviewer', formData).subscribe({
            next: (response) => {
                console.log('Response:', response);
                this.router.navigate(['/login']);
            },
            error: (errorResponse) => {
                console.error('Error:', errorResponse);  // Log the full error response
                if (errorResponse.error.errors) {
                    console.log('Validation Errors:', errorResponse.error.errors);
                }
            }
        });
    } else {
        console.log('Form is invalid');
    }
}

  

    public onProfilePictureUploaded(fileUrl: string) {
        this.selectedFileUrl = fileUrl; // Update the profilePicture field with the uploaded file URL
    }

}
