import { bootstrapApplication } from '@angular/platform-browser';
import { App } from './app/app';
import { provideRouter } from '@angular/router';
import { routes } from './app/app.routes';
import { provideStore } from '@ngrx/store';
import { userReducer } from './app/state/reducers/user.reducer';

bootstrapApplication(App, {
  providers: [
    provideRouter(routes),
    provideStore({ userState: userReducer })
  ]
});
