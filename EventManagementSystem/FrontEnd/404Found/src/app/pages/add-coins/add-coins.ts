import { Component, OnInit } from '@angular/core';

import { FormsModule } from '@angular/forms';
import { WalletService } from '../../services/wallet/wallet';

@Component({
  selector: 'app-add-coins',
  templateUrl: './add-coins.html',
  styleUrl: './add-coins.css',
  imports: [FormsModule],
})
export class AddCoins implements OnInit {
  user: any;
  coins: number = 0;
  amount: number = 0;

  constructor(private walletService: WalletService) {
    walletService.coins$.subscribe({
      next: (coins) => (this.coins = coins),
    });
  }

  ngOnInit() {
    this.walletService.getUserCoins();
  }

  payAndAddCoins() {
    this.walletService.topupCoinsToWallet(this.amount);
  }
}
