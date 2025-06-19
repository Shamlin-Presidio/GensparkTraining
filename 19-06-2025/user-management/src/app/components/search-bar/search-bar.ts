import { Component, ElementRef, ViewChild, inject, AfterViewInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { fromEvent } from 'rxjs';
import { debounceTime, distinctUntilChanged, switchMap } from 'rxjs/operators';
import { UserService } from '../../services/user';
import { User } from '../../models/user.model';

@Component({
  selector: 'app-search-bar',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './search-bar.html',
  styleUrl:'./search-bar.css'
})
export class SearchBar implements AfterViewInit {

  private userService = inject(UserService);
  @ViewChild('searchInput') searchInput!: ElementRef;
  
  filteredUsers: User[] = [];

  ngAfterViewInit() {
    fromEvent(this.searchInput.nativeElement, 'input')
      .pipe(
        debounceTime(500),
        distinctUntilChanged(),
        switchMap(() =>
          this.userService.searchUsers(this.searchInput.nativeElement.value)
        )
      )
      .subscribe(users => {
        this.filteredUsers = users;
      });
  }
}
