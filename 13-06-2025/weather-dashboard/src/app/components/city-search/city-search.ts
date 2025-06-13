import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { WeatherService } from '../../services/weather';

@Component({
  selector: 'app-city-search',
  imports: [FormsModule],
  templateUrl: './city-search.html',
  styleUrl: './city-search.css'
})
export class CitySearch {
  city: string = '';

  constructor(private weatherService: WeatherService) {}

  onSearch(): void {
    if (this.city.trim()) {
      this.weatherService.updateCity(this.city.trim());
      this.city = '';
    }
  }
}
