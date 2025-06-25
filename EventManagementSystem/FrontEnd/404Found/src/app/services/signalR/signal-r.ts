import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { BehaviorSubject } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class SignalR {
  private connection!: signalR.HubConnection;

  private _notifications: any[] = [];
  private _newEvent = new BehaviorSubject<any>(null);
  private _notificationCount = new BehaviorSubject<number>(this.getStoredCount());

  public newEvent$ = this._newEvent.asObservable();
  public notificationCount$ = this._notificationCount.asObservable();

  constructor() {
    this.startConnection();
  }

  private startConnection() {
    this.connection = new signalR.HubConnectionBuilder()
      .withUrl('http://localhost:5025/eventHub', { withCredentials: true })
      .withAutomaticReconnect()
      .build();

    this.connection.on('NewEventCreated', (event) => {
      this._notifications.unshift(event);
      const count = this._notificationCount.value + 1;
      this.setStoredCount(count);
      this._notificationCount.next(count);
      this._newEvent.next(event);
    });

    this.connection.start().catch(err => console.error('SignalR failed:', err));
  }

  markNotificationAsRead() {
    const count = this._notificationCount.value;
    if (count > 0) {
      const updated = count - 1;
      this.setStoredCount(updated);
      this._notificationCount.next(updated);
    }
  }

  resetNotificationCount() {
    this.setStoredCount(0);
    this._notificationCount.next(0);
  }

  private getStoredCount(): number {
    return parseInt(localStorage.getItem('notificationCount') || '0', 10);
  }

  private setStoredCount(count: number) {
    localStorage.setItem('notificationCount', count.toString());
  }
}
