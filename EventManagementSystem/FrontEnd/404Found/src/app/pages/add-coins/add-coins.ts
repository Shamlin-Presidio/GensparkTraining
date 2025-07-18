import { Component, OnInit } from '@angular/core';
import { Auth } from '../../services/auth/auth';
import { HttpClient } from '@angular/common/http';

import { FormsModule } from '@angular/forms';
import { PaymentService } from '../../services/payment/payment';

@Component({
  selector: 'app-add-coins',
  templateUrl: './add-coins.html',
  styleUrl:'./add-coins.css',
  imports: [FormsModule]
})
export class AddCoins implements OnInit {
  user: any;
  coins: number = 0;
  amount: number = 0;

  constructor(private auth: Auth, private paymentService: PaymentService, private http: HttpClient) {}

  ngOnInit() {
    this.user = this.auth.currentUser;
    this.loadCurrentCoins();
  }

  loadCurrentCoins() {
    this.paymentService.getCoins(this.user.id).subscribe({
      next: (coins) => {
        this.coins = coins;
      },
      error: () => {
        this.coins = 0;
      }
    });
  }
  
  // payAndAddCoins() {
  //   this.paymentService.initiatePayment(this.amount, this.user);
  // }
  payAndAddCoins() {
    this.paymentService.initiatePayment(this.amount, (coinsAdded: number) => {
      console.log(`${coinsAdded} coins added successfully`);
      this.user.coins += coinsAdded;
    });
  }
}
