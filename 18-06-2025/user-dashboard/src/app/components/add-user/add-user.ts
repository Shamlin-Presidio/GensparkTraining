import { Component } from '@angular/core';
import { FormGroup, FormControl, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { UserService } from '../../services/user';

@Component({
  selector: 'app-add-user',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './add-user.html',
  styleUrl: './add-user.css'
})
export class AddUser {
  userForm = new FormGroup({
    firstName: new FormControl('', Validators.required),
    lastName: new FormControl('', Validators.required),
    age: new FormControl('', Validators.required),
    gender: new FormControl(''),
    role: new FormControl(''),
    state: new FormControl('')
  });

  constructor(private userService: UserService) {}

  submit() {
    if (this.userForm.valid) {
      this.userService.addUser(this.userForm.value).subscribe(console.log);
      this.userForm.reset();
    }
  }

}
