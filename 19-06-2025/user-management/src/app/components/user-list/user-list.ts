import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserService } from '../../services/user';
import { User } from '../../models/user.model';
import { AsyncPipe } from '@angular/common';

@Component({
  selector: 'app-user-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './user-list.html'
})
export class UserList {
  userService = inject(UserService);
  users$ = this.userService.users$;

  ngOnInit() {
  this.users$.subscribe(users => {
    console.log('Users in list:', users);
  });
}


}

