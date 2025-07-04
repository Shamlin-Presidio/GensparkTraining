import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule, FormsModule } from '@angular/forms';

@Component({
  selector: 'app-payment-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, FormsModule],
  templateUrl: './payment-form.html',
  styleUrls: ['./payment-form.css']
})
export class PaymentForm {
  form: FormGroup;
  paymentStatus: 'success' | 'failed' | 'cancelled' | null = null;
  paymentId: string = '';

  constructor(private fb: FormBuilder) {
    this.form = this.fb.group({
      name: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      contact: ['', [Validators.required, Validators.pattern(/^\d{10}$/)]],
      amount: ['', [Validators.required, Validators.min(1)]]
    });
  }

  onSubmit() {
    if (this.form.invalid) return;

    const options: any = {
      key: 'rzp_test_YourKeyHere',
      amount: +this.form.value.amount! * 100,
      currency: 'INR',
      name: this.form.value.name,
      description: 'Test Payment',
      handler: (response: any) => {
        this.paymentStatus = 'success';
        this.paymentId = response.razorpay_payment_id;
      },
      prefill: {
        name: this.form.value.name,
        email: this.form.value.email,
        contact: this.form.value.contact,
        method: 'upi',
        upi: {
          flow: 'collect',
          vpa: 'success@razorpay'
        }
      },
      theme: {
        color: '#3399cc'
      },
      modal: {
        ondismiss: () => {
          this.paymentStatus = 'cancelled';
        }
      }
    };

    const rzp = new (window as any).Razorpay(options);
    rzp.open();
  }
}
