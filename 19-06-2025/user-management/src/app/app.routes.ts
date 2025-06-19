import { Routes } from '@angular/router';
import { UserForm } from './components/user-form/user-form';
import { UserList } from './components/user-list/user-list';
import { SearchBar } from './components/search-bar/search-bar';

export const routes: Routes = [
  { path: '', component: UserForm },
  { path: 'list', component: UserList },
  { path: 'search', component: SearchBar },
];
