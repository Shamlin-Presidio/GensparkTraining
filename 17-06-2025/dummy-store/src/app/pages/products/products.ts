import { Component, HostListener, OnInit } from '@angular/core';
import { debounceTime, distinctUntilChanged, switchMap } from 'rxjs';
import { ProductService } from '../../services/product';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ProductCard } from '../../shared/product-card/product-card';

@Component({
  selector: 'app-products',
  imports: [CommonModule, ReactiveFormsModule, ProductCard],
  templateUrl: './products.html',
  styleUrl: './products.css'
})
export class Products implements OnInit {
  products: any[] = [];
  searchControl = new FormControl('');
  skip = 0;
  loading = false;
  noResults = false;

  constructor(private productService: ProductService) {}

  ngOnInit(): void {
    this.searchControl.valueChanges.pipe(
      debounceTime(500),
      distinctUntilChanged(),
      switchMap((query) => {
        this.skip = 0;
        this.products = [];
        return this.productService.searchProducts(query || '', 0);
      })
    ).subscribe((res) => {
      this.products = res.products;
      this.noResults = this.products.length === 0;
    });

    this.searchControl.setValue('');
  }

  @HostListener('window:scroll')
  onScroll(): void {
    if ((window.innerHeight + window.scrollY) >= document.body.offsetHeight - 2 && !this.loading) {
      this.loading = true;
      this.skip += 10;
      const query = this.searchControl.value || '';
      this.productService.searchProducts(query, this.skip).subscribe((res) => {
        this.products = [...this.products, ...res.products];
        this.loading = false;
      });
    }
  }
}
