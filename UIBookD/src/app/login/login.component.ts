import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClient, provideHttpClient } from '@angular/common/http';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
  standalone: true,  // Mark the component as standalone
  imports: [ReactiveFormsModule, CommonModule]  // Import ReactiveFormsModule here
})
export class LoginComponent {
  loginForm: FormGroup;
  

  constructor(private fb: FormBuilder, private router: Router, private http: HttpClient) {
    console.log('LoginComponent Constructor');
    this.loginForm = this.fb.group({
      email: ['', [Validators.required]],
      password: ['', [Validators.required]]
    });
  }
  
  ngOnInit(): void {
    console.log('LoginComponent ngOnInit');
  }


  onLogin(event: Event) {
    event.preventDefault();
  
    console.log('Form Status:', this.loginForm.status);
    console.log('Form Errors:', this.loginForm.errors);
    console.log('Email Errors:', this.loginForm.get('email')?.errors);
    console.log('Password Errors:', this.loginForm.get('password')?.errors);

    if (this.loginForm.valid) {
      const formData = this.loginForm.value;
  
      this.http.post('https://localhost:7267/api/User/login', formData).subscribe((response: any) => {
        console.log('Response:', response);
        console.log('Token:', response.token);
        console.log('Id:', response.userId);
  
        localStorage.setItem('authToken', response.token);
        
        localStorage.setItem('UserId', response.userId);  // Ensure userId is included in the response

        this.router.navigate(['/dashboard']);
      }, error => {
        console.log('Login error:', error);
      });
    } else {
      console.log('Form is invalid');
    }
  }
  
}
