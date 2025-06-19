import { createReducer, on } from '@ngrx/store';
import { addUser, setSearchQuery } from '../actions/user.actions';
import { User } from '../../models/user.model';

export interface UserState {
  users: User[];
  searchQuery: string;
}

const initialState: UserState = {
  users: [],
  searchQuery: ''
};

export const userReducer = createReducer(
  initialState,
  on(addUser, (state, { user }) => ({
    ...state,
    users: [...state.users, user]
  })),
  on(setSearchQuery, (state, { query }) => ({
    ...state,
    searchQuery: query
  }))
);
