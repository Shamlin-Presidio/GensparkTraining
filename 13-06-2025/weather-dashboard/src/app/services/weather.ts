import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, catchError, throwError, timer, switchMap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class WeatherService {
  private readonly API_KEY = '5f9c5a9b2f57789af814ee1a7e9b7c23'; 
  private readonly API_URL = 'https://api.openweathermap.org/data/2.5/weather';

  private citySubject = new BehaviorSubject<string>(''); 
  city$ = this.citySubject.asObservable();

  constructor(private http: HttpClient) {}


  updateCity(city: string): void {
    this.citySubject.next(city);
  }

  getWeather(city: string): Observable<any> {
    const url = `${this.API_URL}?q=${city}&appid=${this.API_KEY}&units=metric`;

    return this.http.get(url).pipe(
      catchError((err) => {
        let msg = 'An error occurred fetching weather data';
        if (err.status === 404) msg = 'City not found';
        return throwError(() => new Error(msg));
      })
    );
  }

  getLiveWeather(city: string): Observable<any> {
    return timer(0, 300000).pipe( // 5 minutes
      switchMap(() => this.getWeather(city))
    );
  }
}
