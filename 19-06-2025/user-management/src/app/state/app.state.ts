import { User } from '../models/user.model';

export interface AppState {
  userState: {
    users: User[];
    searchQuery: string;
  };
}