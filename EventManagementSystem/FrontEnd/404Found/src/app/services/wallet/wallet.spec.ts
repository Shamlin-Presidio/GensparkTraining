import { TestBed } from '@angular/core/testing';
import {
  HttpClientTestingModule,
  HttpTestingController,
} from '@angular/common/http/testing';
import { WalletService } from './wallet';
import { Auth } from '../auth/auth';
import { BehaviorSubject } from 'rxjs';
import { HttpErrorResponse } from '@angular/common/http';

declare let window: any;

describe('Wallet Service', () => {
  let service: WalletService;
  let httpMock: HttpTestingController;
  let mockAuth: jasmine.SpyObj<Auth>;
  window.Razorpay = jasmine
    .createSpy('Razorpay')
    .and.callFake((options: any) => {
      return {
        open: jasmine.createSpy('open'),
        handler: options.handler,
      };
    });

  beforeEach(() => {
    const authSpy = jasmine.createSpyObj('Auth', [], {
      currentUser: {
        id: 'test-user-id',
        email: 'test@example.com',
      },
    });
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [WalletService, { provide: Auth, useValue: authSpy }],
    });

    service = TestBed.inject(WalletService);
    httpMock = TestBed.inject(HttpTestingController);
    mockAuth = TestBed.inject(Auth) as jasmine.SpyObj<Auth>;

    const RazorpayConstructor = function (this: any, options: any) {
      this.open = jasmine.createSpy('open');
      this.handler = options.handler;
      this.options = options;
    };

    window.Razorpay = jasmine
      .createSpy('Razorpay', RazorpayConstructor as any)
      .and.callThrough();
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  describe('getUserCoins', () => {
    it('should update user coins', () => {
      let emittedCoins: number | undefined;
      service.coins$.subscribe((value) => (emittedCoins = value));

      service.getUserCoins();

      const req = httpMock.expectOne(
        'http://localhost:5025/api/Wallet/GetUserCoins/test-user-id'
      );
      expect(req.request.method).toBe('GET');

      req.flush(150);
      expect(emittedCoins).toBe(150);
    });

    it('should log the error if request fails', () => {
      const consoleSpy = spyOn(console, 'log');

      service.getUserCoins();
      const req = httpMock.expectOne(
        'http://localhost:5025/api/Wallet/GetUserCoins/test-user-id'
      );
      expect(req.request.method).toBe('GET');

      req.flush(
        { message: 'Network error occurred' },
        {
          status: 0,
          statusText: 'Unknown Error',
        }
      );

      expect(consoleSpy).toHaveBeenCalledWith(jasmine.any(HttpErrorResponse));
    });
  });

  describe('topupCoinsToWallet', () => {
    it('should trigger Razorpay and handle payment', () => {
      const alertSpy = spyOn(window, 'alert');

      let emittedCoins: number | undefined;
      service.coins$.subscribe((c) => (emittedCoins = c));

      service.topupCoinsToWallet(200);

      // ✅ Get instance and trigger handler manually
      const lastCallArgs = (window.Razorpay as jasmine.Spy).calls.mostRecent()
        .args[0];
      const instance = new (window.Razorpay as any)(lastCallArgs);
      instance.options.handler({ payment_id: 'mock123' });

      const req = httpMock.expectOne(
        'http://localhost:5025/api/Wallet/TopupCoins/test-user-id?coins=200'
      );
      req.flush(200);

      expect(emittedCoins).toBe(200);
      expect(alertSpy).toHaveBeenCalledWith(
        'Payment successful! The wallet will be updated in a few minutes'
      );
    });
  });

  describe('getTransactionHistory', () => {
    it('should return transaction history', () => {
      const mockTransactions = [
        { id: 1, type: 'Topup', amount: 100, date: new Date().toISOString() },
        {
          id: 2,
          type: 'Registration',
          amount: 50,
          date: new Date().toISOString(),
        },
      ];

      service.getTransactionHistory().subscribe((transactions) => {
        expect(transactions).toEqual(mockTransactions);
      });

      const req = httpMock.expectOne(
        'http://localhost:5025/api/Wallet/Transactions/test-user-id'
      );
      expect(req.request.method).toBe('GET');

      req.flush(mockTransactions);
    });
  });

  describe('getEventWithdawDetails', () => {
    it('should return event withdraw details', () => {
      const mockResponse = [{ id: 1, title: 'Event 1', isWithdrawn: false }];
      localStorage.setItem('accessToken', 'mock-token');

      service.getEventWithdawDetails().subscribe((res) => {
        expect(res).toEqual(mockResponse);
      });

      const req = httpMock.expectOne(
        'http://localhost:5025/api/Wallet/Withdraw/List'
      );
      expect(req.request.method).toBe('GET');
      expect(req.request.headers.get('Authorization')).toBe(
        'Bearer mock-token'
      );

      req.flush(mockResponse);
      localStorage.removeItem('accessToken');
    });
  });

  describe('withdrawEventCoins', () => {
    it('should withdraw event coins', () => {
      const mockResponse = { message: 'Withdraw successful' };
      const eventId = 'abc123';
      localStorage.setItem('accessToken', 'mock-token');

      service.withdrawEventCoins(eventId).subscribe((res) => {
        expect(res).toEqual(mockResponse);
      });

      const req = httpMock.expectOne(
        `http://localhost:5025/api/Wallet/Withdraw/${eventId}`
      );
      expect(req.request.method).toBe('POST');
      expect(req.request.body).toBeNull();
      expect(req.request.headers.get('Authorization')).toBe(
        'Bearer mock-token'
      );

      req.flush(mockResponse);
      localStorage.removeItem('accessToken');
    });
  });
});
