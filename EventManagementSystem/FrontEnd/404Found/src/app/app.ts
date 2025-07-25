import { Component, OnInit } from '@angular/core';
import { RouterOutlet, RouterLink, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { Auth } from './services/auth/auth';
import { SignalR } from './services/signalR/signal-r'; 
import { PaymentService } from './services/payment/payment';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, RouterLink, CommonModule],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App implements OnInit {
  protected title = '404Found';
  user = JSON.parse(localStorage.getItem('user') || '{}');
  defaultImage = './assets/default-avatar.png';
  notificationCount = 0; 
  coins: number = 0;

  constructor(
    private auth: Auth,
    private router: Router,
    private paymentService: PaymentService,
    private signalR: SignalR 
  ) {}

  ngOnInit(): void {
    this.signalR.notificationCount$.subscribe(count => {
      this.notificationCount = count;
    });

    if (this.user?.id && this.isLoggedIn) {
      this.paymentService.getCoins(this.user.id).subscribe({
        next: (coinCount) => {
          this.coins = coinCount;
        },
        error: (err) => {
          console.error('Error fetching coins:', err);
        }
      });
    }
  }

  getProfileImage(): string {
    const path = this.user?.profilePicturePath;
    if (!path) return this.defaultImage;
    return path.startsWith('http') ? path : `http://localhost:5025/${path}`;
  }

  get isOrganizer(): boolean {
    return this.auth.role === 'Organizer';
  }

  get isLoggedIn(): boolean {
    return this.auth.isLoggedIn;
  }

  goToProfile() {
    this.router.navigate(['/profile']);
  }

  goToNotifications() {
    this.signalR.resetNotificationCount();
    this.router.navigate(['/notifications']);
  }

  goToCoins(){
    this.router.navigate(['/add-coins']);
  }

  logout() {
    this.auth.logout();
  }
}
