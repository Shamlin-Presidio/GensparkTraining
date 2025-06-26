import { TestBed } from '@angular/core/testing';
import { Auth } from './auth';
import { Router } from '@angular/router';
import { HttpClientTestingModule, HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { provideHttpClient } from '@angular/common/http';

describe('Auth Service', () => {
  let service: Auth;
  let httpMock: HttpTestingController;
  let mockRouter = { navigate: jasmine.createSpy('navigate') };
  // S E T U P 
  beforeEach(()=>{
    TestBed.configureTestingModule({
      providers:[
        Auth,
        provideHttpClient(),
        provideHttpClientTesting(),
        { provide: Router, useValue: mockRouter }
      ]
    });
    service = TestBed.inject(Auth);
    httpMock = TestBed.inject(HttpTestingController)
  });

  afterEach(()=>{
    httpMock.verify();
    localStorage.clear();
  });

  // T E S T S
  it('Should login and return tokens', ()=>{
    const mockResponse = {
      accessToken: 'abc123',
      refreshToken: 'def456',
      user: { username: 'Shamlin', role: 'Organizer' }
    }

    service.login('Shamlin', 'password').subscribe(res => {
      expect(res).toEqual(mockResponse);
    });

    const req = httpMock.expectOne('http://localhost:5025/api/Auth/login');
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual({ username: 'Shamlin', password: 'password' });

    req.flush(mockResponse);
  });

  it('should save auth data to localStorage', () => {
    const response = {
      accessToken: 'xyz',
      refreshToken: '123',
      user: { id: '1', username: 'Test', role: 'Organizer' }
    };

    service.saveAuthData(response);

    expect(localStorage.getItem('accessToken')).toBe('xyz');
    expect(localStorage.getItem('refreshToken')).toBe('123');
    expect(JSON.parse(localStorage.getItem('user')!)).toEqual(response.user);
  });

  it('should clear localStorage and navigate to login on logout', () => {
    localStorage.setItem('accessToken', 'abc');
    service.logout();
    expect(localStorage.getItem('accessToken')).toBeNull();
    expect(mockRouter.navigate).toHaveBeenCalledWith(['/login']);
  });

  it('should update the stored user object in localStorage', () => {
    const oldUser = { id: '1', username: 'Old', role: 'Attendee' };
    const updatedUser = { username: 'New' };

    localStorage.setItem('user', JSON.stringify(oldUser));
    service.updateUserInStorage(updatedUser);

    const newStoredUser = JSON.parse(localStorage.getItem('user')!);
    expect(newStoredUser.username).toBe('New');
    expect(newStoredUser.role).toBe('Attendee');
  });
});
