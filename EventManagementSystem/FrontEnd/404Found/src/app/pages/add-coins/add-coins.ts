import { Component, OnInit } from '@angular/core';

import { FormsModule } from '@angular/forms';
import { WalletService } from '../../services/wallet/wallet';
import { CurrencyPipe, DatePipe } from '@angular/common';
import { Auth } from '../../services/auth/auth';

@Component({
  selector: 'app-add-coins',
  templateUrl: './add-coins.html',
  styleUrl: './add-coins.css',
  imports: [FormsModule, DatePipe, CurrencyPipe],
})
export class AddCoins implements OnInit {
  user: any;
  coins: number = 0;
  amount: number = 0;

  transactions: any[] = [];

  events: any[] = [];

  onWithdraw(index: number) {
    this.walletService.withdrawEventCoins(this.events[index].id).subscribe({
      next: (response) => {
        if (response) {
          alert(
            'Amount withdrawn successfully! Your wallet will be updated soon'
          );
          this.walletService.getUserCoins();
          this.events[index].isWithdrawn = true;
        } else {
          alert('Unable to withdraw coins. Please try again later.');
        }
      },
      error: (err) => console.log(err),
    });
  }

  constructor(private walletService: WalletService, public auth: Auth) {
    walletService.coins$.subscribe({
      next: (coins) => (this.coins = coins),
    });
  }

  ngOnInit() {
    this.walletService.getUserCoins();
    this.auth.currentUser.role == 'Attendee'
      ? this.loadTransactionHistory()
      : this.loadWithdrawDetails();
  }

  cannotWithdraw(endTime: string) {
    const endDate = new Date(endTime);
    const today = new Date();
    const diffInMs = today.getTime() - endDate.getTime();
    const diffInDays = diffInMs / (1000 * 60 * 60 * 24);

    return diffInDays < 5;
  }

  payAndAddCoins() {
    this.walletService.topupCoinsToWallet(this.amount);
  }

  loadWithdrawDetails() {
    this.walletService.getEventWithdawDetails().subscribe({
      next: (response: any) => {
        this.events = response;
      },
      error: (err) => console.log(err),
    });
  }

  loadTransactionHistory() {
    this.walletService.getTransactionHistory().subscribe({
      next: (response: any) => {
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
