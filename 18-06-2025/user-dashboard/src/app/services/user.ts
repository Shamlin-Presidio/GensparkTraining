import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class UserService {
  private dummyjsonUrl = 'https://dummyjson.com/users';

  constructor(private http: HttpClient) {}

  getUsers(): Observable<any> {
    return this.http.get(`${this.dummyjsonUrl}`);
  }

  addUser(user: any): Observable<any> {
    return this.http.post(`${this.dummyjsonUrl}/add`, user);
  }
}
