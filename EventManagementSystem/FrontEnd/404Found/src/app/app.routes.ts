import { Routes } from '@angular/router';
import { Layout } from './components/layout/layout';
import { Home } from './pages/home/home';
import { Login } from './pages/login/login';
import { Register } from './pages/register/register';
import { EventDetails } from './pages/event-details/event-details';
import {MyRegistrations} from './pages/my-registrations/my-registrations'
import { authGuard } from './guards/auth-guard';
import { guestGuard } from './guards/guest-guard';
import { CreateEvent } from './pages/create-event/create-event';
import { organizerGuard }from './guards/role-guard-guard';
import { MyEvents } from './pages/my-events/my-events';
import { Notifications } from './pages/notification/notification';


export const routes: Routes = [
  {
    path: '',
    component: Layout,
    children: [
      { path: '', component: Home },
      {
        path: 'create-event',
        canActivate: [organizerGuard],
        loadComponent: () =>
          import('./pages/create-event/create-event').then(m => m.CreateEvent),
      },
      {
        path: 'my-events',
        canActivate: [organizerGuard],
        loadComponent: () =>
          import('./pages/my-events/my-events').then(m => m.MyEvents),
      },
        {
        path: 'edit-event/:id',
        canActivate: [organizerGuard],
        loadComponent: () =>
        import('./pages/edit-event/edit-event').then(m => m.EditEvent),
      },
      {
        path: 'profile',
        canActivate: [authGuard],
        loadComponent: () =>
          import('./pages/profile/profile').then(m => m.Profile),
      },
      {
        path: 'notifications',
        canActivate: [authGuard],
        loadComponent: () =>
          import('./pages/notification/notification').then(m => m.Notifications),
      },
    ]
  },
  { path: 'login', component: Login, canActivate: [guestGuard]  },
  { path: 'register', component: Register, canActivate: [guestGuard]  },
  { path: 'event/:id', component: EventDetails },
  {path: 'my-registrations', component: MyRegistrations},
  // { path: 'notifications', component: Notifications },
  { path: '**', redirectTo: '' }
];
