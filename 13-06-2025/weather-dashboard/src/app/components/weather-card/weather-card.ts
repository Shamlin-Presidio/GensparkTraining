import { Component, Input } from '@angular/core';
import { NgIf, TitleCasePipe } from '@angular/common';

@Component({
  selector: 'app-weather-card',
  imports: [NgIf, TitleCasePipe],
  templateUrl: './weather-card.html',
  styleUrl: './weather-card.css'
})
export class WeatherCard {
  @Input() weather!: any;
}