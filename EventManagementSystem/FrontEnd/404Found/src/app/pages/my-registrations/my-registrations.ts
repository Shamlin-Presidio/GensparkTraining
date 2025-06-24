import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Event } from '../../services/event/event';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-my-registrations',
  imports: [CommonModule, RouterModule],
  templateUrl: './my-registrations.html',
  styleUrl: './my-registrations.css'
})

export class MyRegistrations implements OnInit {
  registrations: any[] = [];

  constructor(private eventService: Event) {}

  ngOnInit(): void {
    this.eventService.getMyRegistrations().subscribe(res => {
      this.registrations = res;
    });
  }

  cancelRegistration(registrationId: string) {
    if (!confirm('Are you sure you want to cancel?')) return;
    this.eventService.cancelRegistration(registrationId).subscribe({
      next: () => {
        this.registrations = this.registrations.filter(r => r.id !== registrationId);
      },
      error: err => {
        alert(err.error?.message || 'Cancellation failed');
      }
    });
  }
}