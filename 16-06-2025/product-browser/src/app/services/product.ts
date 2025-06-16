import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';


@Injectable({
  providedIn: 'root'
})


export class ProductService {
  private api = 'https://dummyjson.com/products/search';

  constructor(private http: HttpClient) {}

  searchProducts(query: string, limit: number, skip: number): Observable<any> {
    const url = `${this.api}?q=${query}&limit=${limit}&skip=${skip}`;
    return this.http.get(url);
  }
}