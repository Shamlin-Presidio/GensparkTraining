import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { Auth } from '../../services/auth/auth';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './register.html',
  styleUrls: ['./register.css']
})
export class Register {
  username = '';
  password = '';
  email = '';
  role = 'Attendee';
  profilePicture: File | null = null;
  error: string | null = null;

  constructor(private auth: Auth, private router: Router) { }

  onFileSelected(event: any) {
    this.profilePicture = event.target.files[0];
  }

  register() {
    this.error = null;

    if (!this.username || !this.email || !this.password || !this.role) {
      this.error = 'All fields are required.';
      return;
    }

    const formData = new FormData();
    formData.append('Username', this.username);
    formData.append('Password', this.password);
    formData.append('Email', this.email);
    formData.append('Role', this.role);
    if (this.profilePicture) {
      formData.append('profilePicture', this.profilePicture);
    }

    this.auth.register(formData).subscribe({
      next: (res) => {
        this.auth.saveAuthData(res);
        this.router.navigate(['/']);
      },
      error: (err) => {
        this.error = err.error?.message || 'Registration failed.';
      }
    });
  }

}
