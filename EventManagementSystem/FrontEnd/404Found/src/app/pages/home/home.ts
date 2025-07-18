import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Event as EventService } from '../../services/event/event';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-home',
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './home.html',
  styleUrl: './home.css'
})

export class Home implements OnInit {
  events: any[] = [];
  searchTerm = '';
  currentPage = 1;
  filterDate: string = '';

  constructor(private event : EventService) {}

  ngOnInit(): void {
    this.loadEvents();
  }

  loadEvents() {
    this.event.getEvents(this.searchTerm,this.filterDate, this.currentPage, 10).subscribe((res) => {
      this.events = res.events;
    });
  }
  handleImageError(event: any) {
  const imgElement = event.target as HTMLImageElement;
  imgElement.src = 'assets/default-event.png';
}
onImgError(event: any) {
  event.target.src = 'assets/default-event.png';
}

  goToPage(page: number) {
    if (page < 1) return;
    this.currentPage = page;
    this.loadEvents();
  }
}

