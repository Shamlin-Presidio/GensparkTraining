// import { Injectable } from '@angular/core';
// import * as signalR from '@microsoft/signalr';
// import { Subject } from 'rxjs';

// @Injectable({ providedIn: 'root' })
// export class SignalR {
//   private hubConnection!: signalR.HubConnection;
//   private eventSubject = new Subject<any>();

//   public newEvent$ = this.eventSubject.asObservable();

//   constructor() {
//     this.hubConnection = new signalR.HubConnectionBuilder()
//       .withUrl('http://localhost:5025/eventHub', { withCredentials: true })
//       .withAutomaticReconnect()
//       .build();

//     this.hubConnection.on('NewEventCreated', (event: any) => {
//       this.eventSubject.next(event);
//     });

//     this.hubConnection
//       .start()
//       .then(() => console.log('SignalR Connected'))
//       .catch(err => console.error('SignalR Connection Error:', err));
//   }
// }


import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { Subject } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class SignalR {
  private hubConnection!: signalR.HubConnection;
  public newEvent$ = new Subject<any>();

  constructor() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('http://localhost:5025/eventHub', {
        withCredentials: true
      })
      .withAutomaticReconnect()
      .build();

    this.hubConnection.start()
      .then(() => console.log('SignalR connected!'))
      .catch(err => console.error('SignalR connection failed:', err));

    this.hubConnection.on('NewEventCreated', (eventDto: any) => {
      console.log('New event received via SignalR:', eventDto);
      this.newEvent$.next(eventDto);
    });
  }
}
