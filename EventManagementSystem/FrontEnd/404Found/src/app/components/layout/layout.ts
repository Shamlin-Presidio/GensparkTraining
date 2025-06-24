import { Component } from '@angular/core';
import { RouterOutlet, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { Auth } from '../../services/auth/auth';

@Component({
  selector: 'app-layout',
  imports: [CommonModule, RouterOutlet],
  templateUrl: './layout.html',
  styleUrl: './layout.css'
})

export class Layout {

  constructor(private auth: Auth, private router: Router) {}
  
  user = JSON.parse(localStorage.getItem('user') || '{}');

  get isOrganizer(): boolean {
    return this.auth.role === 'Organizer';
  }

  get isLoggedIn(): boolean {
    return this.auth.isLoggedIn;
  }

  logout() {
    this.auth.logout();
  }
}
