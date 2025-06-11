import { Component } from '@angular/core';
import { First } from "./first/first";
import { ProductList } from "./components/product-list/product-list";
import { CustomerDetails } from "./components/customer-details/customer-details";
import { Product } from "./product/product";

@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  styleUrl: './app.css',
  imports:[First, ProductList, CustomerDetails, Product]
})
export class App {
  protected title = 'myApp';
}