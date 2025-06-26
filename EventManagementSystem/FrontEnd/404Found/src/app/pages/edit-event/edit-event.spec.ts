import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { ActivatedRoute } from '@angular/router';
import { of } from 'rxjs';

import { EditEvent } from './edit-event';

describe('EditEvent', () => {
  let component: EditEvent;
  let fixture: ComponentFixture<EditEvent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EditEvent],
      providers: [
        provideHttpClient(),
        {
          provide: ActivatedRoute,
          useValue: {
            paramMap: of({ get: (key: string) => '123' }),
            snapshot: {
              paramMap: {
                get: (key: string) => '123'
              }
            }
          }
        }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(EditEvent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
