import { ComponentFixture, TestBed } from '@angular/core/testing';
import { EventDetails } from './event-details';
import { provideRouter } from '@angular/router';
import { ActivatedRoute } from '@angular/router';
import { of } from 'rxjs';
import { HttpClientTestingModule } from '@angular/common/http/testing';

describe('EventDetails', () => {
  let component: EventDetails;
  let fixture: ComponentFixture<EventDetails>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EventDetails, HttpClientTestingModule],
      providers: [
        provideRouter([]),
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

    fixture = TestBed.createComponent(EventDetails);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
