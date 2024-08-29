import { Component } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';


@Component({
  selector: 'app-add-book-modal',
  standalone: true,
  templateUrl: './add-book-modal.component.html',
  styleUrl: './add-book-modal.component.css',
  imports: [
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    CommonModule,
    MatDatepickerModule,
    MatNativeDateModule]
})
export class AddBookModalComponent {

    reviewForm: FormGroup;

  constructor(
    private dialogRef: MatDialogRef<AddBookModalComponent>,
    private fb: FormBuilder
  ) {
    this.reviewForm = this.fb.group({
        title: ['', Validators.required],
        author: ['', Validators.required],
        genre: ['', Validators.required],
        description: ['', Validators.required],
        publicationDate: [null, Validators.required],
        publisher: ['', Validators.required],
        numberOfPages: [null, Validators.required],
        isbn: ['', Validators.required],
        language: ['', Validators.required],
        image: [''] // Include image field if necessary, adjust validation if required
    });
  }

  onSubmit(): void {
    if (this.reviewForm.valid) {
        const formValue = this.reviewForm.value;
  
        // Ensure publicationDate is in the correct format
        if (formValue.publicationDate instanceof Date) {
          formValue.publicationDate = formValue.publicationDate.toISOString(); // Convert to ISO string
        }
  
        // Handle image if required by the API
        // Ensure image is properly formatted or handled
  
        this.dialogRef.close(formValue);
      }
  }

  onCancel(): void {
    this.dialogRef.close();
  }
}
