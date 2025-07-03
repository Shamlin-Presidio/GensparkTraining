import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class User {
  private baseUrl = 'http://localhost:5025/api/User';
  // private baseUrl = 'http://localhost:8080/api/User'; // Container

  constructor(private http: HttpClient) {}

  getUserById(userId: string): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}/GetUserById/${userId}`);
  }

  updateUser(userId: string, formData: FormData): Observable<any> {
    return this.http.put(
      `${this.baseUrl}/UpdateUser/${userId}`,
      formData,
      { headers: {
          Authorization: `Bearer ${localStorage.getItem('accessToken') || ''}`
        } 
      }
    );
  }

  deleteUser(userId: string): Observable<any> {
    return this.http.delete(
      `${this.baseUrl}/DeleteUser/${userId}`,
      { headers: {
          Authorization: `Bearer ${localStorage.getItem('accessToken') || ''}`
        } 
      }
    );
  }

  getAllUsers(): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/GetAllUsers`);
  }
}
