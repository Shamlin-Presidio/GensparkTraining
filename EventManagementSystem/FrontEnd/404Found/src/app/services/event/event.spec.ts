import { TestBed } from '@angular/core/testing';
import { Event } from './event';
import { provideHttpClient } from '@angular/common/http';
import {
  provideHttpClientTesting,
  HttpTestingController,
} from '@angular/common/http/testing';

describe('Event Service', () => {
  let service: Event;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [Event, provideHttpClient(), provideHttpClientTesting()],
    });

    service = TestBed.inject(Event);
    httpMock = TestBed.inject(HttpTestingController);
    localStorage.clear();
    localStorage.setItem('accessToken', 'test-token');
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should fetch events', () => {
    const mockEvents = { events: [], total: 0 };

    service.getEvents('tech', '', 1, 5).subscribe((res) => {
      expect(res).toEqual(mockEvents);
    });

    const req = httpMock.expectOne(
      'http://localhost:5025/api/Event/GetEvents/?search=tech&page=1&pageSize=5'
    );
    expect(req.request.method).toBe('GET');
    req.flush(mockEvents);
  });

  it('should fetch event by ID', () => {
    const mockEvent = { id: '1', title: 'Mock Event' };

    service.getEventById('1').subscribe((res) => {
      expect(res).toEqual(mockEvent);
    });

    const req = httpMock.expectOne(
      'http://localhost:5025/api/Event/GetEventById/1'
    );
    expect(req.request.method).toBe('GET');
    req.flush(mockEvent);
  });

  it('should register for event', () => {
    const mockResponse = { success: true };

    service.registerForEvent('123').subscribe((res) => {
      expect(res).toEqual(mockResponse);
    });

    const req = httpMock.expectOne(
      'http://localhost:5025/api/Registration/Register/123'
    );
    expect(req.request.method).toBe('POST');
    expect(req.request.headers.get('Authorization')).toBe('Bearer test-token');
    req.flush(mockResponse);
  });

  it('should get my registrations', () => {
    const mockRegs = [{ eventId: '1' }];

    service.getMyRegistrations().subscribe((res) => {
      expect(res).toEqual(mockRegs);
    });

    const req = httpMock.expectOne(
      'http://localhost:5025/api/Registration/GetMyRegistrations'
    );
    expect(req.request.method).toBe('GET');
    expect(req.request.headers.get('Authorization')).toBe('Bearer test-token');
    req.flush(mockRegs);
  });

  it('should cancel registration', () => {
    service.cancelRegistration('abc').subscribe();

    const req = httpMock.expectOne(
      'http://localhost:5025/api/Registration/Cancel/abc'
    );
    expect(req.request.method).toBe('DELETE');
    expect(req.request.headers.get('Authorization')).toBe('Bearer test-token');
    req.flush({});
  });

  it('should get registration count', () => {
    const mockCount = 5;

    service.getRegistrationsCount('abc').subscribe((res) => {
      expect(res).toBe(mockCount);
    });

    const req = httpMock.expectOne(
      'http://localhost:5025/api/Registration/Count/abc'
    );
    expect(req.request.method).toBe('GET');
    req.flush(mockCount);
  });

  it('should create event', () => {
    const formData = new FormData();
    formData.append('title', 'New Event');

    const mockEvent = { id: '1', title: 'New Event' };

    service.createEvent(formData).subscribe((res) => {
      expect(res).toEqual(mockEvent);
    });

    const req = httpMock.expectOne(
      'http://localhost:5025/api/Event/CreateEvent'
    );
    expect(req.request.method).toBe('POST');
    expect(req.request.headers.get('Authorization')).toBe('Bearer test-token');
    req.flush(mockEvent);
  });

  it('should get my events', () => {
    const myEvents = [{ id: '1', title: 'My Event' }];

    service.getMyEvents().subscribe((res) => {
      expect(res).toEqual(myEvents);
    });

    const req = httpMock.expectOne('http://localhost:5025/api/Event/MyEvents');
    expect(req.request.method).toBe('GET');
    expect(req.request.headers.get('Authorization')).toBe('Bearer test-token');
    req.flush(myEvents);
  });

  it('should delete event', () => {
    service.deleteEvent('123').subscribe();

    const req = httpMock.expectOne(
      'http://localhost:5025/api/Event/DeleteEvent/123'
    );
    expect(req.request.method).toBe('DELETE');
    expect(req.request.headers.get('Authorization')).toBe('Bearer test-token');
    req.flush({});
  });

  it('should update event', () => {
    const formData = new FormData();
    formData.append('title', 'Updated Event');

    const updatedEvent = { id: '123', title: 'Updated Event' };

    service.updateEvent('123', formData).subscribe((res) => {
      expect(res).toEqual(updatedEvent);
    });

    const req = httpMock.expectOne(
      'http://localhost:5025/api/Event/UpdateEvent/123'
    );
    expect(req.request.method).toBe('PUT');
    expect(req.request.headers.get('Authorization')).toBe('Bearer test-token');
    req.flush(updatedEvent);
  });
});
