// import { TestBed } from '@angular/core/testing';
// import { SignalR } from './signal-r';
// import * as signalR from '@microsoft/signalr';

// describe('SignalR Service', () => {
//   let service: SignalR;

//   const mockHubConnection = jasmine.createSpyObj('HubConnection', ['start', 'on', 'invoke']);
//   mockHubConnection.start.and.returnValue(Promise.resolve());

//   beforeEach(() => {

//     spyOn(signalR, 'HubConnectionBuilder').and.returnValue({
//       withUrl: () => ({
//         withAutomaticReconnect: () => ({
//           build: () => mockHubConnection
//         })
//       })
//     } as any);


//     localStorage.clear();


//     TestBed.configureTestingModule({});
//     service = TestBed.inject(SignalR);
//   });

//   it('should create the service', () => {
//     expect(service).toBeTruthy();
//   });

//   it('should initialize connection and set up event handler', () => {
//     expect(mockHubConnection.start).toHaveBeenCalled();
//     expect(mockHubConnection.on).toHaveBeenCalledWith(
//       'NewEventCreated',
//       jasmine.any(Function)
//     );
//   });

//   it('should increment and persist notification count when event received', (done) => {
//     const mockEvent = {
//       id: 'abc123',
//       title: 'New Event',
//       description: 'An event description',
//       imagePath: 'path/to/image.jpg'
//     };

//     const callback = mockHubConnection.on.calls.argsFor(0)[1];
//     callback(mockEvent);

//     service.notificationCount$.subscribe(count => {
//       expect(count).toBe(1);
//       expect(localStorage.getItem('notificationCount')).toBe('1');
//       done();
//     });
//   });

//   it('should decrease count when markNotificationAsRead is called', (done) => {
//     localStorage.setItem('notificationCount', '2');
//     service['notificationCount$'].subscribe(); 
//     service.markNotificationAsRead();

//     service.notificationCount$.subscribe(count => {
//       expect(count).toBe(1);
//       expect(localStorage.getItem('notificationCount')).toBe('1');
//       done();
//     });
//   });

//   it('should reset count to 0', (done) => {
//     localStorage.setItem('notificationCount', '3');
//     service.resetNotificationCount();

//     service.notificationCount$.subscribe(count => {
//       expect(count).toBe(0);
//       expect(localStorage.getItem('notificationCount')).toBe('0');
//       done();
//     });
//   });
// });
