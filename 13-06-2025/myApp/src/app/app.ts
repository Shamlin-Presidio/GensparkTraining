import { Component } from '@angular/core';
import { First } from "./first/first";
import { CustomerDetails } from "./components/customer-details/customer-details";
import { Recipes } from "./recipes/recipes";

@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  styleUrl: './app.css',
  imports:[First, CustomerDetails, Recipes]
})
export class App {
  protected title = 'myApp';
}