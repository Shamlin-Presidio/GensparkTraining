import { Component } from '@angular/core';
import { Login } from './components/login/login';

@Component({
  selector: 'app-root',
  imports: [Login],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected title = 'login-app';
}
