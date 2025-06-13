import { Component } from '@angular/core';
import { WeatherDashboard } from './components/weather-dashboard/weather-dashboard';
import { CitySearch } from './components/city-search/city-search';
import { WeatherCard } from './components/weather-card/weather-card';
import { TitleCasePipe } from '@angular/common';


@Component({
  selector: 'app-root',
  standalone: true,
  imports: [ TitleCasePipe, WeatherDashboard, CitySearch, WeatherCard],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected title = 'weather-dashboard';
}