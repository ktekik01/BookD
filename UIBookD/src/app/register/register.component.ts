import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClient, provideHttpClient } from '@angular/common/http';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
  standalone: true,  // Mark the component as standalone
  imports: [ReactiveFormsModule, CommonModule,]  // Import ReactiveFormsModule here
})
export class RegisterComponent {
  registerForm: FormGroup;

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

      console.log('Form Data:', this.registerForm.value);

      //API call to register user
        this.http.post('https://localhost:7267/api/User/reviewer', this.registerForm.value).subscribe((response) => {
        console.log('Response:', response);
        this.router.navigate(['/login']);
        });
    } else {
      console.log('Form is invalid');
    }
  }
}
