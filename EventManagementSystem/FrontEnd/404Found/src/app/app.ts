import { Component } from '@angular/core';
import { RouterOutlet,RouterLink, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { Auth } from './services/auth/auth';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, RouterLink, CommonModule],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected title = '404Found';
  constructor(private auth: Auth, private router: Router) {}
  
  user = JSON.parse(localStorage.getItem('user') || '{}');
  defaultImage = './assets/default-avatar.png';

  get isOrganizer(): boolean {
    return this.auth.role === 'Organizer';
  }

  get isLoggedIn(): boolean {
    return this.auth.isLoggedIn;
  }

  goToProfile() {
    this.router.navigate(['/profile']);
  }

  logout() {
    this.auth.logout();
  }
}
