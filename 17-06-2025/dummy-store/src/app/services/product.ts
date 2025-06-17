import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';


@Injectable({ providedIn: 'root' })
export class ProductService {
  private API_URL = 'https://dummyjson.com/products';

  constructor(private http: HttpClient) {}

  searchProducts(query: string, skip: number = 0, limit: number = 10): Observable<any> {
    return this.http.get(`${this.API_URL}/search?q=${query}&limit=${limit}&skip=${skip}`);
  }

  getProductById(id: number): Observable<any> {
    return this.http.get(`${this.API_URL}/${id}`);
  }
}