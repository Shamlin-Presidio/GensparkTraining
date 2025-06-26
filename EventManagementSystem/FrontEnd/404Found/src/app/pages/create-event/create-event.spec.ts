import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';

import { CreateEvent } from './create-event';

describe('CreateEvent', () => {
  let component: CreateEvent;
  let fixture: ComponentFixture<CreateEvent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CreateEvent],
      providers: [provideHttpClient()]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CreateEvent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
