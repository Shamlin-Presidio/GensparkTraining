declare interface RazorpayOptions {
  key: string;
  amount: number;
  currency: string;
  name: string;
  description: string;
  handler: (response: { razorpay_payment_id: string }) => void;
  prefill?: {
    name?: string;
    email?: string;
    contact?: string;
    method?: string;
    upi?: {
      flow?: string;
      vpa?: string;
    };
  };
  theme?: {
    color?: string;
  };
  modal?: {
    ondismiss?: () => void;
  };
}

declare class Razorpay {
  constructor(options: RazorpayOptions);
  open(): void;
}
