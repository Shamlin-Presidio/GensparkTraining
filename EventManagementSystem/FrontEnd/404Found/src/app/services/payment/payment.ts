import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Auth } from '../auth/auth';
import { Observable } from 'rxjs';

declare var Razorpay: any;

@Injectable({
  providedIn: 'root'
})
export class PaymentService {
  private apiUrl = 'http://localhost:5025/api/User';

  constructor(private http: HttpClient, private auth: Auth) {}

  getCoins(userId: string): Observable<number> {
    return this.http.get<number>(`${this.apiUrl}/${userId}/coins`);
  }

  initiatePayment(amount: number, onSuccess: (coinsAdded: number) => void) {
    const user = this.auth.currentUser;

    const options: any = {
      key: 'rzp_test_I1bHELIBcoH8Ph',
      amount: amount * 100,
      currency: 'INR',
      name: '404 Found',
      description: 'Add Coins',
      handler: (response: any) => {
        const coinsToAdd = amount;
        const newTotal = Number(user.coins) + Number(coinsToAdd);

        // console.log('newTotal:', newTotal, 'user.coins:', user.coins, 'coinsToAdd:', coinsToAdd);

        this.http.put(`${this.apiUrl}/${user.id}/coins?coins=${coinsToAdd}`, null).subscribe(() => {
          this.auth.updateUserInStorage({ coins: newTotal });
          onSuccess(coinsToAdd);
        });
      },
      prefill: {
        email: user.email || '',
        contact: '9999999999'
      },
      theme: {
        color: '#3399cc'
      }
    };

    const rzp = new Razorpay(options);
    rzp.open();
  }
}
