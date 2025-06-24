import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { Event } from '../../services/event/event';
import { Auth } from '../../services/auth/auth';
import { HttpClient } from '@angular/common/http';


@Component({
  selector: 'app-event-details',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './event-details.html',
  styleUrl: './event-details.css'
})
export class EventDetails implements OnInit {
  event: any;
  isRegistered = false;
  isOrganizer = false;
  registrationId: string | null = null;
  registrationCount: number = 0;

  constructor(
    private route: ActivatedRoute,
    private eventService: Event,
    public auth: Auth,
    private http: HttpClient
  ) {}

  ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id');
    if (!id) return;

    this.eventService.getEventById(id).subscribe(res => {
      this.event = res;
      this.isOrganizer = this.auth.role === 'Organizer';
      this.checkIfRegistered(res.id);

      // No.  O F   R E G I S T R A T I O N S
      this.eventService.getRegistrationsCount(res.id).subscribe(resp => {
      this.registrationCount = resp.count;
    });
    });
  }

  checkIfRegistered(eventId: string) {
    this.eventService.getMyRegistrations().subscribe(res => {
      const reg = res.find((r: any) => r.eventId === eventId);
      this.isRegistered = !!reg;
      this.registrationId = reg?.id || null;
    });
  }

  fetchRegistrationCount(eventId: string) {
    this.eventService.getRegistrationsCount(eventId).subscribe(resp => {
      this.registrationCount = resp.count;
    });
  }


  register() {
    this.eventService.registerForEvent(this.event.id).subscribe({
      next: (res) => {
        this.isRegistered = true;
        this.registrationId = res.id;
        alert('Registered successfully!');

        this.fetchRegistrationCount(this.event.id);
      },
      error: (err) => {
        alert(err.error?.message || 'Registration failed');
      }
    });
  }

  cancelRegistration() {
    if (!this.registrationId) {
      alert('You are not registered or registrationId is missing.');
      return;
    }

    this.eventService.cancelRegistration(this.registrationId).subscribe({
      next: (res) => {
        this.isRegistered = false;
        this.registrationId = null;
        alert(res.message || 'Registration cancelled.');

        this.fetchRegistrationCount(this.event.id);
      },
      error: (err) => {
        alert(err.error?.message || 'Cancellation failed.');
      }
    });
  }
}
