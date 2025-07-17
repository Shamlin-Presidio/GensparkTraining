import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddCoins } from './add-coins';

describe('AddCoins', () => {
  let component: AddCoins;
  let fixture: ComponentFixture<AddCoins>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddCoins]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddCoins);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
