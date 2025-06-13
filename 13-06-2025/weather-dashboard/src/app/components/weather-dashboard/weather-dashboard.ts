import { Component, OnInit } from '@angular/core';
import { AsyncPipe, NgIf } from '@angular/common';
import { WeatherService } from '../../services/weather';
import { WeatherCard } from '../weather-card/weather-card';
import { Observable, switchMap, of } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';

@Component({
  selector: 'app-weather-dashboard',
  imports: [NgIf, AsyncPipe, WeatherCard],
  templateUrl: './weather-dashboard.html',
  styleUrl: './weather-dashboard.css'
})
export class WeatherDashboard implements OnInit {
  weather$!: Observable<any>;
  errorMessage: string | null = null;
  loading: boolean = false;

  constructor(private weatherService: WeatherService) {}

  ngOnInit(): void {
    this.weather$ = this.weatherService.city$.pipe(
      tap(() => {
        this.errorMessage = null;
        this.loading = true;
      }),
      switchMap((city: string) => {
        if (!city) return of(null); 
        return this.weatherService.getWeather(city).pipe(
          tap(() => this.loading = false),
          catchError((err) => {
            this.loading = false;
            this.errorMessage = err.message || 'Error fetching weather';
            return of(null);
          })
        );
      })
    );
  }
}