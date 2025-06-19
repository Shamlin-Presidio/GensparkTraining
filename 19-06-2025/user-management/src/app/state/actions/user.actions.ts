import { createAction, props } from '@ngrx/store';
import { User } from '../../models/user.model';

export const addUser = createAction('[User] Add User', props<{ user: User }>());
export const setSearchQuery = createAction('[User] Set Search Query', props<{ query: string }>());