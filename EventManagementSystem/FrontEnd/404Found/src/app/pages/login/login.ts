import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { Auth } from '../../services/auth/auth';

@Component({
  selector: 'app-login',
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class Login {
  username = '';
  password = '';
  error: string | null = null;


  constructor(private auth: Auth, private router: Router) { }

  // login() {
  //   this.auth.login(this.username, this.password).subscribe({
  //     next: (res) => {
  //       this.auth.saveAuthData(res);
  //       this.router.navigate(['/']);
  //     },
  //     error: (err) => {
  //       this.error = 'Login failed. Check credentials.';
  //       console.error(err);
  //     }
  //   });
  // }
  login() {
    if (!this.username || !this.password) {
      this.error = 'Username and password are required';
      return;
    }

    this.auth.login(this.username, this.password).subscribe({
      next: (res) => {
        this.auth.saveAuthData(res);

        const returnUrl = localStorage.getItem('returnUrl');
        localStorage.removeItem('returnUrl');

        if (returnUrl) {
          this.router.navigateByUrl(returnUrl);
        } else {
          this.router.navigate(['/']);
        }
      },
      error: (err) => {
        this.error = err.error?.message || 'Login failed';
      }
    });
  }

}
