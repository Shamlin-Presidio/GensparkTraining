import { Routes } from '@angular/router';
import { AddUser } from './components/add-user/add-user';
import { UserDashboard } from './components/user-dashboard/user-dashboard';

export const routes: Routes = [
  { path: '', component: UserDashboard },
  { path: 'add', component: AddUser }
];
