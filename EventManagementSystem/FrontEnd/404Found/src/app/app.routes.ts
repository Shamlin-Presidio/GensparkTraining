import { Routes } from '@angular/router';
import { Layout } from './components/layout/layout';
import { Home } from './pages/home/home';
import { Login } from './pages/login/login';
import { Register } from './pages/register/register';
import { EventDetails } from './pages/event-details/event-details';
import {MyRegistrations} from './pages/my-registrations/my-registrations'
import { authGuard } from './guards/auth-guard';
import { guestGuard } from './guards/guest-guard';


export const routes: Routes = [
  {
    path: '',
    component: Layout,
    children: [
      { path: '', component: Home },
    //   {
    //     path: 'create-event',
    //     canActivate: [authGuard],
    //     loadComponent: () =>
    //       import('./pages/create-event/create-event').then(m => m.CreateEvent),
    //   },
    //   {
    //     path: 'profile',
    //     canActivate: [authGuard],
    //     loadComponent: () =>
    //       import('./pages/profile/profile').then(m => m.Profile),
    //   },
    ]
  },
  { path: 'login', component: Login, canActivate: [guestGuard]  },
  { path: 'register', component: Register, canActivate: [guestGuard]  },
  { path: 'event/:id', component: EventDetails },
  {path: 'my-registrations', component: MyRegistrations},
  { path: '**', redirectTo: '' }
];
