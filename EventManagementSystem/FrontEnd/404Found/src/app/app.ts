import { Component, OnInit } from '@angular/core';
import { RouterOutlet, RouterLink, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { Auth } from './services/auth/auth';
import { SignalR } from './services/signalR/signal-r'; 

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

  constructor(
    private auth: Auth,
    private router: Router,
    private signalR: SignalR 
  ) {}

  ngOnInit(): void {
    this.signalR.notificationCount$.subscribe(count => {
      this.notificationCount = count;
    });
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
    this.signalR.resetNotificationCount(); // 👈 Reset badge
    this.router.navigate(['/notifications']);
  }

  logout() {
    this.auth.logout();
  }
}
