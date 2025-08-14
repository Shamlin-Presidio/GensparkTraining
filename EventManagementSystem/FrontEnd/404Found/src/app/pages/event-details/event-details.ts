import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { Event } from '../../services/event/event';
import { Auth } from '../../services/auth/auth';
import { HttpClient } from '@angular/common/http';
import { WalletService } from '../../services/wallet/wallet';

@Component({
  selector: 'app-event-details',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './event-details.html',
  styleUrl: './event-details.css',
})
export class EventDetails implements OnInit {
  event: any;
  isRegistered = false;
  isOrganizer = false;
  registrationId: string | null = null;
  registrationCount: number = 0;
  attendees: any[] = [];
  showAttendees = false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private eventService: Event,
    public auth: Auth,
    private wallet: WalletService
  ) {}

  isExpiredEvent = false;

  ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id');
    if (!id) return;

    this.eventService.getEventById(id).subscribe((res) => {
      this.event = res;
      this.isOrganizer = this.auth.role === 'Organizer';
      this.checkIfRegistered(res.id);

      // Normalize both dates to ignore time when comparing
      const today = new Date();
      const eventStart = new Date(res.startTime);

      today.setHours(0, 0, 0, 0);
      eventStart.setHours(0, 0, 0, 0);

      this.isExpiredEvent = eventStart <= today;

      this.eventService.getRegistrationsCount(res.id).subscribe((resp) => {
        this.registrationCount = resp.count;
      });

      this.getAttendees(res.id);
    });
  }

  checkIfRegistered(eventId: string) {
    this.eventService.getMyRegistrations().subscribe((res) => {
      const reg = res.find((r: any) => r.eventId === eventId);
      this.isRegistered = !!reg;
      this.registrationId = reg?.id || null;
    });
  }

  fetchRegistrationCount(eventId: string) {
    this.eventService.getRegistrationsCount(eventId).subscribe((resp) => {
      this.registrationCount = resp.count;
    });
  }

  getAttendees(eventId: string) {
    this.eventService.getRegisteredAttendees(eventId).subscribe({
      next: (res) => {
        this.attendees = res;
      },
      error: (err) => {
        alert(err.error?.message || 'Failed to fetch attendees');
      },
    });
  }

  register() {
    if (!this.auth.isLoggedIn) {
      const returnUrl = this.router.url;
      localStorage.setItem('returnUrl', returnUrl);
      this.router.navigate(['/login']);
      return;
    }
    if (
      confirm(
        `Registering for this event will cost ${this.event.registrationFee} coins. Do you want to register for this event?`
      )
    ) {
      this.eventService.registerForEvent(this.event.id).subscribe({
        next: (res) => {
          this.isRegistered = true;
          this.registrationId = res.id;
          alert('Registered successfully!');
          this.fetchRegistrationCount(this.event.id);
          this.wallet.getUserCoins();
        },
        error: (err) => {
          alert(err.error?.message || 'Registration failed');
        },
      });
    }
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
        this.wallet.getUserCoins();
      },
      error: (err) => {
        alert(err.error?.message || 'Cancellation failed.');
      },
    });
  }
  onImgError(event: any) {
  event.target.src = 'assets/default-event.png';
}
}
