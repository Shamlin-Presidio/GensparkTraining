import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { Auth } from '../auth/auth';

declare var Razorpay: any;

@Injectable({
  providedIn: 'root',
})
export class WalletService {
  private baseUrl = 'http://localhost:5025/api/Wallet';
  coins = new BehaviorSubject<number>(0);
  coins$ = this.coins.asObservable();

  constructor(private http: HttpClient, private auth: Auth) {}

  getUserCoins() {
    const userId = this.auth.currentUser.id;
    this.http.get(`${this.baseUrl}/GetUserCoins/${userId}`).subscribe({
      next: (coins: any) => this.coins.next(coins),
      error: (error) => console.log(error),
    });
  }

  topupCoinsToWallet(coins: number) {
    const userId = this.auth.currentUser.id;
    const email = this.auth.currentUser.email;

    const options: any = {
      key: 'rzp_test_I1bHELIBcoH8Ph',
      amount: coins * 100,
      currency: 'INR',
      name: '404 Found',
      description: 'Add Coins',
      handler: (response: any) => {
        alert(
          'Payment successful! The wallet will be updated in a few minutes'
        );
        this.http
          .put(`${this.baseUrl}/TopupCoins/${userId}?coins=${coins}`, null)
          .subscribe({
            next: (coins: any) => this.coins.next(coins),
          });
      },
      prefill: {
        email: email || '',
        contact: '9999999999',
      },
      theme: {
        color: '#3399cc',
      },
    };

    const rzp = new Razorpay(options);
    rzp.open();
  }

  getTransactionHistory() {
    const userId = this.auth.currentUser.id;
    return this.http.get(`${this.baseUrl}/Transactions/${userId}`);
  }

  getEventWithdawDetails() {
    return this.http.get(`${this.baseUrl}/Withdraw/List`, {
      headers: {
        Authorization: `Bearer ${localStorage.getItem('accessToken') || ''}`,
      },
    });
  }

  withdrawEventCoins(eventId: string) {
    return this.http.post(`${this.baseUrl}/Withdraw/${eventId}`, null, {
      headers: {
        Authorization: `Bearer ${localStorage.getItem('accessToken') || ''}`,
      },
    });
  }
}
