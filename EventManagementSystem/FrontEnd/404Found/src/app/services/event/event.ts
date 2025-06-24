import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({ providedIn: 'root' })
export class Event {
  private apiUrl = 'http://localhost:5025/api/Event';

  constructor(private http: HttpClient) {}

  getEvents(search = '', page = 1, pageSize = 10) {
    let url = `${this.apiUrl}/GetEvents/?`;

    if (search) url += `search=${search}&`;
    url += `page=${page}&pageSize=${pageSize}`;

    return this.http.get<any>(url);
  }
  getEventById(id: string) {
    return this.http.get<any>(`${this.apiUrl}/GetEventById/${id}`);
  }

  registerForEvent(eventId: string) {
    return this.http.post<any>(
      `${this.apiUrl.replace('/Event', '')}/Registration/Register/${eventId}`,
        {},
      {
        headers: {
          Authorization: `Bearer ${localStorage.getItem('accessToken')}`
        }
      }
    );
  } 

  getMyRegistrations() {
    return this.http.get<any>(
      'http://localhost:5025/api/Registration/GetMyRegistrations',
      {
        headers: {
          Authorization: `Bearer ${localStorage.getItem('accessToken') || ''}`
        }
      }
    );
  }
  cancelRegistration(registrationId: string) {
    return this.http.delete<any>(
    `http://localhost:5025/api/Registration/Cancel/${registrationId}`,
    {
      headers: {
        Authorization: `Bearer ${localStorage.getItem('accessToken') || ''}`
      }
    }
    );
  }

  getRegistrationsCount(eventId: string){
    return this.http.get<any>(
    `http://localhost:5025/api/Registration/Count/${eventId}`
  );
  }

}
