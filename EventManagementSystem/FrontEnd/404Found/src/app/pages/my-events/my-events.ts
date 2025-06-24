import { Component, OnInit } from '@angular/core';
import { Event } from '../../services/event/event';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-my-events',
  templateUrl: './my-events.html',
  imports: [CommonModule],
  styleUrls: ['./my-events.css']
})
export class MyEvents implements OnInit {
  events: any[] = [];

  constructor(private eventService: Event, private router: Router) {}

  // ngOnInit() {
  //   this.eventService.getMyEvents().subscribe({
  //     next: res => this.events = res,
  //     error: err => alert(err.error?.message || 'Failed to fetch events')
  //   });
  // }

  ngOnInit() {
  this.eventService.getMyEvents().subscribe({
    next: res => this.events = res,
    error: err => {
        if (err.status === 403) {
          alert('You are not authorized to view this page.');
          this.router.navigate(['/']); 
        } else {
          alert(err.error?.message || 'Failed to fetch events');
        }
      }
    });
  }


  editEvent(id: string) {
    this.router.navigate(['/edit-event', id]);
  }

  deleteEvent(id: string) {
  if (!confirm('Are you sure you want to delete this event?')) return;

  this.eventService.deleteEvent(id).subscribe({
    next: () => {
        this.events = this.events.filter(e => e.id !== id);
        alert('Event deleted successfully.');
      },
      error: err => {
        alert(err.error?.message || 'Deletion failed.');
      }
    });
  }
}
