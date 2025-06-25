import { Component, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
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

  constructor(private signalRService: SignalR) {}

  ngOnInit(): void {
    this.signalRService.newEvent$.subscribe((event: any) => {
      this.notifications.unshift(event);
    });
  }
}
