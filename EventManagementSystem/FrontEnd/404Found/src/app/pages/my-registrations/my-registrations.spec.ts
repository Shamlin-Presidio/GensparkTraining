import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MyRegistrations } from './my-registrations';
import { provideHttpClient } from '@angular/common/http';

describe('MyRegistrations', () => {
  let component: MyRegistrations;
  let fixture: ComponentFixture<MyRegistrations>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MyRegistrations],
      providers: [provideHttpClient()]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MyRegistrations);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
