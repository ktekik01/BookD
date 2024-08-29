import { Component } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';


@Component({
  selector: 'app-add-review-modal',
  standalone: true,
  templateUrl: './add-review-modal.component.html',
  styleUrls: ['./add-review-modal.component.css'],
  imports: [    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
]
})
export class AddReviewModalComponent {
  reviewForm: FormGroup;

  constructor(
    private dialogRef: MatDialogRef<AddReviewModalComponent>,
    private fb: FormBuilder
  ) {
    this.reviewForm = this.fb.group({
      title: ['', Validators.required],
      reviewText: ['', Validators.required],
      bookTitle: ['', [Validators.required]]
    });
  }

  onSubmit(): void {
    if (this.reviewForm.valid) {
      // Send data to the parent component or API
      this.dialogRef.close(this.reviewForm.value);
    }
  }

  onCancel(): void {
    this.dialogRef.close();
  }
}
