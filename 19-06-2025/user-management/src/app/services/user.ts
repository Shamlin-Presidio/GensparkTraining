import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { User } from '../models/user.model';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private usersSubject = new BehaviorSubject<User[]>([]);
  users$ = this.usersSubject.asObservable();

  addUser(user: User) {
    const currentUsers = this.usersSubject.value;
    this.usersSubject.next([...currentUsers, user]);
  }

  searchUsers(query: string): Observable<User[]> {
    const q = query.toLowerCase();
    return new Observable(subscriber => {
      const filtered = this.usersSubject.value.filter(user =>
        user.username.toLowerCase().includes(q) ||
        user.role.toLowerCase().includes(q)
      );
      subscriber.next(filtered);
    });
  }
}
