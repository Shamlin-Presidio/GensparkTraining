import { Component, OnInit } from '@angular/core';

import { FormsModule } from '@angular/forms';
import { WalletService } from '../../services/wallet/wallet';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-add-coins',
  templateUrl: './add-coins.html',
  styleUrl: './add-coins.css',
  imports: [FormsModule, DatePipe],
})
export class AddCoins implements OnInit {
  user: any;
  coins: number = 0;
  amount: number = 0;

  transactions: any[] = [];

  constructor(private walletService: WalletService) {
    walletService.coins$.subscribe({
      next: (coins) => (this.coins = coins),
    });
  }

  ngOnInit() {
    this.walletService.getUserCoins();
    this.loadTransactionHistory();
  }

  payAndAddCoins() {
    this.walletService.topupCoinsToWallet(this.amount);
  }

  loadTransactionHistory() {
    this.walletService.getTransactionHistory().subscribe({
      next: (response: any) => {
        console.log(response);
        this.transactions = response;
      },
      error: (err) => console.log(err),
    });
  }

  formatDateForInput(dateStr: string): string {
    const date = new Date(dateStr);

    const pad = (n: any) => String(n).padStart(2, '0');
    const year = date.getFullYear();
    const month = pad(date.getMonth() + 1);
    const day = pad(date.getDate());
    const hours = pad(date.getHours());
    const minutes = pad(date.getMinutes());

    return `${year}-${month}-${day}T${hours}:${minutes}`;
  }
}
