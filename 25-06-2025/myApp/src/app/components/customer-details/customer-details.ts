import { Component } from '@angular/core';

@Component({
  selector: 'app-customer-details',
  imports: [],
  templateUrl: './customer-details.html',
  styleUrl: './customer-details.css'
})
export class CustomerDetails {
  likes = 0;
  dislikes = 0;

  like() {
    this.likes++;
  }

  dislike() {
    this.dislikes++;
  }
}
