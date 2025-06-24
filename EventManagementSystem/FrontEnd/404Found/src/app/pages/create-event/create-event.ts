import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Event } from '../../services/event/event';
import { Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-create-event',
  templateUrl: './create-event.html',
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  styleUrls: ['./create-event.css']
})
export class CreateEvent {
  eventForm: FormGroup;
  selectedFile: File | null = null;

  constructor(
    private fb: FormBuilder,
    private eventService: Event,
    private router: Router
  ) {
    this.eventForm = this.fb.group({
      title: ['', Validators.required],
      description: [''],
      startTime: ['', Validators.required],
      endTime: ['', Validators.required],
      location: [''],
      googleMapLink: [''],
      onlineMeetUrl: [''],
      image: [null]
    });
  }

  onFileSelected(event: any) {
    this.selectedFile = event.target.files[0];
  }

  submit() {
    if (this.eventForm.invalid) return;

    const formData = new FormData();
    const formValue = this.eventForm.value;

    const startTimeUTC = new Date(formValue.startTime).toISOString();
    const endTimeUTC = new Date(formValue.endTime).toISOString();

    formData.append('Title', formValue.title);
    formData.append('Description', formValue.description);
    formData.append('StartTime', startTimeUTC);
    formData.append('EndTime', endTimeUTC);
    formData.append('Location', formValue.location);
    formData.append('GoogleMapLink', formValue.googleMapLink);
    formData.append('OnlineMeetUrl', formValue.onlineMeetUrl);

    if (this.selectedFile) {
      formData.append('ImagePath', this.selectedFile, this.selectedFile.name);
    }

    this.eventService.createEvent(formData).subscribe({
      next: res => {
        alert('Event created successfully!');
        this.router.navigate(['/']);
      },
      error: err => {
        alert(err.error?.message || 'Event creation failed');
      }
    });
  }
}
