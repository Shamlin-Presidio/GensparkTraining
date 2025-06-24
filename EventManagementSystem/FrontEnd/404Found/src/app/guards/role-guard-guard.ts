import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { Auth } from '../services/auth/auth';

export const organizerGuard: CanActivateFn = () => {
  const auth = inject(Auth);
  const router = inject(Router);

  if (auth.isLoggedIn && auth.role === 'Organizer') {
    return true;
  } else {
    alert('You are not authorized to access this page.');
    router.navigate(['/']);
    return false;
  }
};
