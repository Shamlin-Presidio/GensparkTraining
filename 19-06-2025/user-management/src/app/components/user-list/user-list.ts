import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Store } from '@ngrx/store';
import { AppState } from '../../state/app.state';
import { selectAllUsers } from '../../state/selectors/user.selector';
import { AsyncPipe } from '@angular/common';

@Component({
  selector: 'app-user-list',
  standalone: true,
  imports: [CommonModule, AsyncPipe],
  templateUrl: './user-list.html'
})
export class UserList {
  private store = inject(Store<AppState>);
  users$ = this.store.select(selectAllUsers);
}
