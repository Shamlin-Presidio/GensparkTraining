import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';

import { MyEvents } from './my-events';

describe('MyEvents', () => {
  let component: MyEvents;
  let fixture: ComponentFixture<MyEvents>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MyEvents],
      providers: [provideHttpClient()]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MyEvents);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
