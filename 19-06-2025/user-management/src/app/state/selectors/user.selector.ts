import { createSelector } from '@ngrx/store';
import { AppState } from '../app.state';
import { UserState } from '../reducers/user.reducer';

export const selectUserState = (state: AppState) => state.userState;

export const selectAllUsers = createSelector(
  selectUserState,
  (state: UserState) => state.users
);

export const selectSearchQuery = createSelector(
  selectUserState,
  (state: UserState) => state.searchQuery
);

export const selectFilteredUsers = createSelector(
  selectAllUsers,
  selectSearchQuery,
  (users, query) => {
    const q = query.toLowerCase();
    return users.filter(user =>
      user.username.toLowerCase().includes(q) ||
      user.role.toLowerCase().includes(q)
    );
  }
);
