import { Component, OnInit, HostListener } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { debounceTime, distinctUntilChanged, switchMap, tap } from 'rxjs/operators';
import { ProductService } from '../../services/product';
import { ProductCard } from '../../components/product-card/product-card';


@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, ProductCard],
  templateUrl: './home.html',
  styleUrls: ['./home.css']
})


export class Home implements OnInit {
  searchControl = new FormControl('');
  products: any[] = [];
  isLoading = false;
  limit = 10;
  skip = 0;
  searchTerm = '';

  constructor(private productService: ProductService) {}

  ngOnInit(): void {
    this.searchControl.valueChanges.pipe(
      debounceTime(400),
      distinctUntilChanged(),
      tap(value => {
        this.products = [];
        this.skip = 0;
        this.searchTerm = value || '';
        this.isLoading = true;
      }),
      switchMap(value => this.productService.searchProducts(value || '', this.limit, this.skip)),
      tap(() => this.isLoading = false)
    ).subscribe((res: any) => {
      this.products = res.products;
    });
  }

  loadMore(): void {
    if (this.isLoading) return;
    this.skip += this.limit;
    this.isLoading = true;
    this.productService.searchProducts(this.searchTerm, this.limit, this.skip).subscribe((res: any) => {
      this.products = [...this.products, ...res.products];
      this.isLoading = false;
    });
  }

  @HostListener('window:scroll', [])
  onScroll(): void {
    const threshold = 300;
    const position = window.innerHeight + window.scrollY;
    const height = document.body.offsetHeight;

    if (position > height - threshold) {
      this.loadMore();
    }
  }
}
