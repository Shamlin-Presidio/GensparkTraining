import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { PaymentForm } from './components/payment-form';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, PaymentForm],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected title = 'razorpay-upi-test';
}
