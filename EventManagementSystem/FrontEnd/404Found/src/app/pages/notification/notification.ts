import { Component, OnInit } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { SignalR } from '../../services/signalR/signal-r';
import { CommonModule } from '@angular/common';


@Component({
  selector: 'app-notifications',
  imports: [RouterLink, CommonModule],
  templateUrl: './notification.html',
  styleUrls: ['./notification.css']
})
export class Notifications implements OnInit {
  notifications: any[] = [];

  constructor(private signalRService: SignalR, private router: Router) {}

  ngOnInit(): void {
    this.signalRService.newEvent$.subscribe((event: any) => {
      if (event) this.notifications.unshift(event);
    });
  }

  viewDetails(id: string) {
    this.signalRService.markNotificationAsRead(); 
    this.router.navigate(['/event', id]);
  }
}
