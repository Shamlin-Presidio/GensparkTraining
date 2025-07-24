import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddCoins } from './add-coins';
import { provideHttpClient } from '@angular/common/http';
import { WalletService } from '../../services/wallet/wallet';
import { of, throwError } from 'rxjs';
import { Auth } from '../../services/auth/auth';
import { FormsModule } from '@angular/forms';
import { By } from '@angular/platform-browser';

describe('AddCoins', () => {
  let component: AddCoins;
  let fixture: ComponentFixture<AddCoins>;
  let walletMock: jasmine.SpyObj<WalletService>;
  let authMock: jasmine.SpyObj<Auth>;

  beforeEach(async () => {
    const walletSpy = jasmine.createSpyObj(
      'WalletService',
      [
        'getUserCoins',
        'topupCoinsToWallet',
        'getTransactionHistory',
        'getEventWithdawDetails',
      ],
      {
        coins$: of(20),
      }
    );

    const authSpy = jasmine.createSpyObj('Auth', [], {
      currentUser: { role: 'Attendee' },
    });

    await TestBed.configureTestingModule({
      imports: [AddCoins, FormsModule],
      providers: [
        provideHttpClient(),
        { provide: WalletService, useValue: walletSpy },
        { provide: Auth, useValue: authSpy },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(AddCoins);
    walletMock = TestBed.inject(WalletService) as jasmine.SpyObj<WalletService>;
    walletMock.getTransactionHistory.and.returnValue(of([]));
    authMock = TestBed.inject(Auth) as jasmine.SpyObj<Auth>;

    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  describe('OnInit', () => {
    it('should load transaction history if role is Attendee', () => {
      authMock.currentUser.role = 'Attendee';
      spyOn(component, 'loadTransactionHistory');
      component.ngOnInit();
      expect(walletMock.getUserCoins).toHaveBeenCalled();
      expect(component.loadTransactionHistory).toHaveBeenCalled();
    });

    it('should load withdraw details if role is Organizer', () => {
      authMock.currentUser.role = 'Organizer';
      spyOn(component, 'loadWithdrawDetails');
      component.ngOnInit();
      expect(walletMock.getUserCoins).toHaveBeenCalled();
      expect(component.loadWithdrawDetails).toHaveBeenCalled();
    });
  });

  describe('Withdraw coins (Organizer)', () => {
    it('should return true if the event ended less than 5 days ago', () => {
      const date = new Date();
      date.setDate(date.getDate() - 3);
      const result = component.cannotWithdraw(date.toISOString());
      expect(result).toBeTrue();
    });

    it('should return false if the event ended 5 or more days ago', () => {
      const date = new Date();
      date.setDate(date.getDate() - 6);
      const result = component.cannotWithdraw(date.toISOString());
      expect(result).toBeFalse();
    });

    it('should return true if the event ends today', () => {
      const today = new Date();
      const result = component.cannotWithdraw(today.toISOString());
      expect(result).toBeTrue();
    });

    it('should enable the Withdraw button if event ended more than 5 days ago', () => {
      authMock.currentUser.role = 'Organizer';
      const date = new Date();
      date.setDate(date.getDate() - 6);
      component.events = [
        {
          endTime: date.toISOString(),
        },
      ];
      fixture.detectChanges();
      const button: HTMLButtonElement =
        fixture.nativeElement.querySelector('button');
      expect(button.disabled).toBeFalse();
    });

    it('should disable the Withdraw button if event ended less than 5 days ago', () => {
      authMock.currentUser.role = 'Organizer';
      const date = new Date();
      date.setDate(date.getDate() - 3);
      component.events = [
        {
          endTime: date.toISOString(),
        },
      ];
      fixture.detectChanges();
      const button: HTMLButtonElement =
        fixture.nativeElement.querySelector('button');
      expect(button.disabled).toBeTrue();
    });
  });

  describe('Top up coins (Attendee)', () => {
    it('should call topupCoinsToWallet() with the correct amount', () => {
      authMock.currentUser.role = 'Attendee';
      component.amount = 500;
      component.payAndAddCoins();
      expect(walletMock.topupCoinsToWallet).toHaveBeenCalledWith(500);
    });

    it('should call payAndAddCoins() when Add Coins button is clicked', () => {
      authMock.currentUser.role = 'Attendee';
      const spy = spyOn(component, 'payAndAddCoins');
      fixture.detectChanges();

      const button = fixture.nativeElement.querySelector('button');
      button.click();

      expect(spy).toHaveBeenCalled();
    });
  });

  describe('Withdraw Table (Organizer', () => {
    it('should render withdraw details when loadWithdrawDetails is called', () => {
      authMock.currentUser.role = 'Organizer';
      const mockResponse = [
        {
          title: 'Event A',
          endTime: new Date().toISOString(),
          totalRegistrations: 50,
          registeredCoins: 100,
          isWithdrawn: false,
        },
        {
          title: 'Event B',
          endTime: new Date().toISOString(),
          totalRegistrations: 30,
          registeredCoins: 60,
          isWithdrawn: true,
        },
      ];

      walletMock.getEventWithdawDetails.and.returnValue(of(mockResponse));

      component.loadWithdrawDetails();
      fixture.detectChanges();

      expect(component.events.length).toBe(2);
      const rows = fixture.debugElement.queryAll(By.css('tbody tr'));
      expect(rows.length).toBe(2);

      const firstRowCells = rows[0].queryAll(By.css('td'));
      expect(firstRowCells[0].nativeElement.textContent.trim()).toBe('Event A');
      expect(firstRowCells[2].nativeElement.textContent.trim()).toBe('50');
      expect(firstRowCells[3].nativeElement.textContent.trim()).toContain(
        '100'
      );
      expect(firstRowCells[4].query(By.css('button'))).toBeTruthy();
      expect(firstRowCells[4].query(By.css('badge'))).toBeFalsy();

      const secondRowCells = rows[1].queryAll(By.css('td'));
      expect(secondRowCells[0].nativeElement.textContent.trim()).toBe(
        'Event B'
      );
      expect(secondRowCells[2].nativeElement.textContent.trim()).toBe('30');
      expect(secondRowCells[3].nativeElement.textContent.trim()).toContain(
        '60'
      );
      expect(secondRowCells[4].query(By.css('button'))).toBeFalsy();
      expect(secondRowCells[4].query(By.css('span'))).toBeTruthy();
    });

    it('should handle error when loadWithdrawDetails fails', () => {
      authMock.currentUser.role = 'Organizer';
      const consoleSpy = spyOn(console, 'log');
      walletMock.getTransactionHistory.and.returnValue(
        throwError(() => new Error('API failed'))
      );

      component.loadTransactionHistory();

      expect(consoleSpy).toHaveBeenCalledWith(jasmine.any(Error));
      expect(component.transactions).toEqual([]);
    });
  });

  describe('should render transaction details when loadTransactionHistory is called', () => {
    it('should render transaction rows with correct class based on type', () => {
      const mockTransactions = [
        {
          id: 'TXN1',
          date: new Date().toISOString(),
          type: 'Topup',
          amount: 100,
        },
        {
          id: 'TXN2',
          date: new Date().toISOString(),
          type: 'Registration',
          amount: 50,
        },
        {
          id: 'TXN3',
          date: new Date().toISOString(),
          type: 'Refund',
          amount: 30,
        },
      ];

      walletMock.getTransactionHistory.and.returnValue(of(mockTransactions));

      component.loadTransactionHistory();
      fixture.detectChanges();

      const rows = fixture.debugElement.queryAll(By.css('tbody tr'));

      // First row: Topup
      expect(rows[0].nativeElement.classList).toContain('table-success');
      expect(rows[0].nativeElement.textContent).toContain('TXN1');

      // Second row: Registration
      expect(rows[1].nativeElement.classList).toContain('table-danger');
      expect(rows[1].nativeElement.textContent).toContain('TXN2');

      // Third row: Refund
      expect(rows[2].nativeElement.classList).toContain('table-warning');
      expect(rows[2].nativeElement.textContent).toContain('TXN3');
    });

    it('should display a message when there are no transactions', () => {
      walletMock.getTransactionHistory.and.returnValue(of([]));

      component.loadTransactionHistory();
      fixture.detectChanges();

      const messageCell = fixture.debugElement.query(By.css('td.text-muted'));
      expect(messageCell).toBeTruthy();
      expect(messageCell.nativeElement.textContent).toContain(
        'No transactions found.'
      );
    });

    it('should handle error when getTransactionHistory fails', () => {
      const consoleSpy = spyOn(console, 'log');
      walletMock.getTransactionHistory.and.returnValue(
        throwError(() => new Error('API failed'))
      );

      component.loadTransactionHistory();

      expect(consoleSpy).toHaveBeenCalledWith(jasmine.any(Error));
      expect(component.transactions).toEqual([]);
    });
  });

  describe('Date format', () => {
    it('should format a valid ISO date string correctly', () => {
      const result = component.formatDateForInput('2024-12-25T08:30:00Z');
      expect(result).toBe('2024-12-25T14:00');
    });

    it('should format a date with single-digit month, day, hour, and minute correctly', () => {
      const result = component.formatDateForInput('2023-03-05T04:07:00');
      expect(result).toBe('2023-03-05T04:07');
    });
  });
});
