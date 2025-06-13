import { Component } from '@angular/core';
import { User } from '../../models/user.model';
import { AuthService } from '../../services/auth';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-login',
  imports: [CommonModule, FormsModule],
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class Login {
  user: User = { username: '', password: '' };
  loginStatus = '';

  constructor(private authService: AuthService) {}

  onLogin() {
    const success = this.authService.login(this.user);
    if (success) {
      this.loginStatus = 'Login successful!';
      this.authService.saveToLocalStorage();
    } else {
      this.loginStatus = 'Invalid username or password.';
    }
  }

  showSessionUser() {
    const user = this.authService.getUserFromSession();
    alert('SessionStorage: ' + JSON.stringify(user));
  }

  showLocalUser() {
    const user = this.authService.getUserFromLocal();
    alert('LocalStorage: ' + JSON.stringify(user));
  }
}
