import { Component } from '@angular/core';
import { First } from "./first/first";
import { ProductList } from "./components/product-list/product-list";
import { CustomerDetails } from "./components/customer-details/customer-details";

@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  styleUrl: './app.css',
  imports:[First, ProductList, CustomerDetails]
})
export class App {
  protected title = 'myApp';
}