import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { Event } from '../../services/event/event';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-edit-event',
  templateUrl: './edit-event.html',
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  styleUrls: ['./edit-event.css']
})
export class EditEvent implements OnInit {
  eventForm: FormGroup;
  selectedFile: File | null = null;
  eventId: string = '';

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
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

  ngOnInit() {
    // const eventId = this.route.snapshot.paramMap.get('id');
    this.eventId = this.route.snapshot.paramMap.get('id') || '';

      this.eventService.getEventById(this.eventId!).subscribe(event => {
      this.eventForm.patchValue({
        title: event.title,
        description: event.description,
        startTime: this.formatDateForInput(event.startTime),
        endTime: this.formatDateForInput(event.endTime),
        location: event.location,
        googleMapLink: event.googleMapLink,
        onlineMeetUrl: event.onlineMeetUrl
      });
    });
  }

  formatDateForInput(dateStr: string): string {
    const date = new Date(dateStr);
    return date.toISOString().slice(0, 16);
  }


  onFileSelected(event: any) {
    this.selectedFile = event.target.files[0];
  }

  submit() {
    if (this.eventForm.invalid || !this.eventId) return;

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

    this.eventService.updateEvent(this.eventId, formData).subscribe({
      next: () => {
        alert('Event updated successfully!');
        this.router.navigate(['/my-events']);
      },
      error: err => {
        alert(err.error?.message || 'Update failed');
      }
    });
  }
}
