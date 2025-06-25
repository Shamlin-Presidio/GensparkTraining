import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

@Injectable({ providedIn: 'root' })
export class Auth {
  private apiUrl = 'http://localhost:5025/api/Auth';

  constructor(private http: HttpClient, private router: Router) {}

  login(username: string, password: string) {
    return this.http.post<any>(`${this.apiUrl}/login`, { username, password });
  }
  register(formData: FormData) {
    return this.http.post<any>(`${this.apiUrl}/signup`, formData);
  }

  saveAuthData(response: any) {
    localStorage.setItem('accessToken', response.accessToken);
    localStorage.setItem('refreshToken', response.refreshToken);
    localStorage.setItem('user', JSON.stringify(response.user));
  }

  logout() {
    localStorage.clear();
    this.router.navigate(['/login']);
  }

  updateUserInStorage(updatedUser: any) {
    const current = this.currentUser;
    if (!current) return;

    const mergedUser = { ...current, ...updatedUser };
    localStorage.setItem('user', JSON.stringify(mergedUser));
  }

  get currentUser() {
    return JSON.parse(localStorage.getItem('user') || 'null');
  }

  get isLoggedIn() {
    return !!localStorage.getItem('accessToken');
  }

  get role() {
    return this.currentUser?.role;
  }
}
