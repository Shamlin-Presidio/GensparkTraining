import { Injectable } from '@angular/core';
import { User } from '../models/user.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private dummyUsers: User[] = [
    { username: 'sham', password: '123' },
    { username: 'lin', password: 'abc' }
  ];

  login(user: User): boolean {
    const found = this.dummyUsers.find(
      u => u.username === user.username && u.password === user.password
    );

    if (found) {
      sessionStorage.setItem('loggedInUser', JSON.stringify(found));
      return true;
    }

    return false;
  }

  getUserFromSession(): User | null {
    const userStr = sessionStorage.getItem('loggedInUser');
    return userStr ? JSON.parse(userStr) : null;
  }

  saveToLocalStorage(): void {
    const sessionUser = sessionStorage.getItem('loggedInUser');
    if (sessionUser) {
      localStorage.setItem('userCopy', sessionUser);
    }
  }

  getUserFromLocal(): User | null {
    const userStr = localStorage.getItem('userCopy');
    return userStr ? JSON.parse(userStr) : null;
  }
  constructor() { }
}
