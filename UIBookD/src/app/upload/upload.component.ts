import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { HttpClient, HttpEventType } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgModule } from '@angular/core';

@Component({
  selector: 'app-upload',
  templateUrl: './upload.component.html',
  styleUrls: ['./upload.component.css'],
  standalone: true,
    imports: [CommonModule, FormsModule]
})
export class UploadComponent implements OnInit {

    public message: string = '';
    public progress: number = 0;
    @Output() public onUploadFinished = new EventEmitter<any>();

    constructor(private http: HttpClient) { }

    ngOnInit(): void { }

    public uploadFile = (files: FileList | null) => {
        if (!files || files.length === 0) {
            return;
        }

        let fileToUpload = files[0]; // No need for casting here
        const formData = new FormData();
        formData.append('file', fileToUpload, fileToUpload.name);

        this.http.post('https://localhost:7267/api/Upload', formData, { reportProgress: true, observe: 'events' }).subscribe(event => {
            if (event.type === HttpEventType.UploadProgress) {
                this.progress = Math.round(100 * (event.loaded / (event.total || 1)));
            } else if (event.type === HttpEventType.Response) {
                this.message = 'Upload success';
                this.onUploadFinished.emit(event.body);
            }
        });
    }

    public uploadProfilePicture = (files: FileList | null) => {
        if (files && files.length > 0) {
            const fileToUpload = files[0];
            const formData = new FormData();
            formData.append('file', fileToUpload, fileToUpload.name);
    
            this.http.post<{ fileUrl: string }>('https://localhost:7267/api/Upload/upload', formData)
                .subscribe(response => {
                    console.log('Uploaded file URL:', response.fileUrl); // Log the URL
                    this.message = 'Upload success';
                    this.onUploadFinished.emit(response.fileUrl); // Emit the file URL
                });
        }
    }
    
    
    
}
