import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-product-list',
  imports: [CommonModule],
  templateUrl: './product-list.html',
  styleUrl: './product-list.css'
})
export class ProductList {
  cartCount = 0;

  products = [
    { name: 'Phone', price:'Rs. 1,50,000', img: 'https://www.apple.com/newsroom/images/2024/09/apple-debuts-iphone-16-pro-and-iphone-16-pro-max/article/Apple-iPhone-16-Pro-hero-geo-240909_inline.jpg.large.jpg' },
    { name: 'Laptop', price:'Rs. 2,50,000', img: 'https://store.storeimages.cdn-apple.com/1/as-images.apple.com/is/mbp16-spaceblack-select-202410?wid=892&hei=820&fmt=jpeg&qlt=90&.v=Nys1UFFBTmI1T0VnWWNyeEZhdDFYamhTSEZFNjlmT2xUUDNBTjljV1BxWjZkZE52THZKR1lubXJyYmRyWWlhOXZvdUZlR0V0VUdJSjBWaDVNVG95YlBROXI4TlIyY1pzUUZwNVlXcEFNb2c'},
    { name: 'Watch', price:'Rs. 90,000', img: 'https://store.storeimages.cdn-apple.com/1/as-images.apple.com/is/MXM83ref_FV99_VW_34FR+watch-case-46-aluminum-rosegold-nc-s10_VW_34FR+watch-face-46-aluminum-rosegold-s10_VW_34FR?wid=750&hei=712&trim=1%2C0&fmt=p-jpg&qlt=95&.v=RS9tdlNZM2NaZ0FSNHV6djJaNGYzVW5TeWJ6QW43NUFnQ2V4cmRFc1VnWWYyNHkrWFJNZ1BodmdwcWlUcmtNMkhaMkVQZTdleWFvVytrdnNBQmJzc2RGNnlaeXQ4NGFKQTAzc0NGeHR2aWJiLzMwazFsQmpWNUowMkIwc3EzL0xpSkl2OTJEMEdGMUpkR2p1bmRlWnppQkl6RktUTzNzK0diVkdSRW9qODJ1cnhrUDFMS3JOVEFCL1ZDcWNJWTR6UzBXSGFyZ1V1dnNnU3BOUVRoSDBMeU00WmFMY0NlY0hQSnl6eVBFcW1tNA' }
  ];

  addToCart() {
    this.cartCount++;
  }
}
