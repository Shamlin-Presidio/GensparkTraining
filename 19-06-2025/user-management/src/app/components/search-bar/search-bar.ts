import { Component, ElementRef, ViewChild, inject, AfterViewInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { fromEvent } from 'rxjs';
import { debounceTime, distinctUntilChanged, map } from 'rxjs/operators';
import { Store } from '@ngrx/store';
import { AppState } from '../../state/app.state';
import { setSearchQuery } from '../../state/actions/user.actions';
import { selectFilteredUsers } from '../../state/selectors/user.selector';
import { AsyncPipe } from '@angular/common';

@Component({
  selector: 'app-search-bar',
  standalone: true,
  imports: [CommonModule, AsyncPipe],
  templateUrl: './search-bar.html',
  styleUrl: './search-bar.css'
})
export class SearchBar implements AfterViewInit {
  @ViewChild('searchInput') searchInput!: ElementRef;
  private store = inject(Store<AppState>);

  filteredUsers$ = this.store.select(selectFilteredUsers);

  ngAfterViewInit() {
    fromEvent(this.searchInput.nativeElement, 'input').pipe(
      debounceTime(300),
      distinctUntilChanged(),
      map((event: any) => event.target.value)
    ).subscribe(query => {
      this.store.dispatch(setSearchQuery({ query }));
    });
  }
}
