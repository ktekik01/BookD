import { Component } from '@angular/core';
import { MatDialogRef} from '@angular/material/dialog';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { MatOptionModule } from '@angular/material/core';

@Component({
  selector: 'app-add-list-modal',
  standalone: true,
  imports: [CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
MatOptionModule,
MatSelectModule,],
  templateUrl: './add-list-modal.component.html',
  styleUrl: './add-list-modal.component.css'
})
export class AddListModalComponent {

    listForm: FormGroup;
    listTypes = ['wishlist', 'reading list']; // Add your list types here

    constructor(
        private dialogRef: MatDialogRef<AddListModalComponent>,
        private fb: FormBuilder
      ) {
        this.listForm = this.fb.group({
          name: ['', Validators.required],
          description: ['', Validators.required],
          type: ['', [Validators.required]]
        });

            // Watch for changes to the list type field to show/hide other fields
    this.listForm.get('type')?.valueChanges.subscribe(value => {
        this.updateFormFieldsVisibility(value);
      });
      }


      updateFormFieldsVisibility(listType: string): void {
        if (listType === 'wishlist') {
          // Hide description field if 'wishlist' is selected
          this.listForm.get('description')?.disable();

          this.listForm.get('name')?.disable()
        } else {
          // Show description field for other types
          this.listForm.get('description')?.enable();

            this.listForm.get('name')?.enable();
        }
      }
      


      onSubmit(): void {
        if (this.listForm.valid) {
          const formValue = this.listForm.value;
          // Process form data here
          console.log('Form submitted:', formValue);
        }
      }
    

      onCancel(): void {
        this.dialogRef.close();
      }


      

}
