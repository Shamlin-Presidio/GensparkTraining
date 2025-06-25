import { Injectable, inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";

@Injectable({ providedIn: 'root' })
export class RecipeService {
  private http = inject(HttpClient);

  getAllRecipes(): Observable<any> {
    return this.http.get('https://dummyjson.com/recipes');
  }
}
