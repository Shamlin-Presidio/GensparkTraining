import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Event } from '../../services/event/event';
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

  constructor(private event : Event) {}

  ngOnInit(): void {
    this.loadEvents();
  }

  loadEvents() {
    this.event.getEvents(this.searchTerm,this.filterDate, this.currentPage, 10).subscribe((res) => {
      this.events = res.events;
    });
  }
  goToPage(page: number) {
    if (page < 1) return;
    this.currentPage = page;
    this.loadEvents();
  }
}

