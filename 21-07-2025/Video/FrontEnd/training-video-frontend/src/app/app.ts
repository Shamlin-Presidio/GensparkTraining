import { Component, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';

interface Video {
  id: number;
  title: string;
  description: string;
  blobUrl: string;
  uploadDate: Date;
}

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected title = 'training-video-frontend';
  protected videos: Video[] = [];

  private http = inject(HttpClient);

  constructor() {
    this.fetchVideos();
  }

  private fetchVideos() {
    this.http.get<Video[]>('http://localhost:5216/api/Videos').subscribe({
      next: (res) => {
        this.videos = res;
      },
      error: (err) => {
        console.error('Error fetching videos:', err);
      }
    });
  }
}
