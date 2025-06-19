import { Component, inject } from '@angular/core';
import { ReactiveFormsModule, FormGroup, FormBuilder, Validators, AbstractControl, ValidationErrors } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { User } from '../../models/user.model';
import { Store } from '@ngrx/store';
import { addUser } from '../../state/actions/user.actions';
import { AppState } from '../../state/app.state';

@Component({
  selector: 'app-user-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './user-form.html',
  styleUrl: './user-form.css'
})
export class UserForm {
  private fb = inject(FormBuilder);
  private store = inject(Store<AppState>);

  bannedWords = ['admin', 'root'];

  bannedUsernameValidator = (control: AbstractControl): ValidationErrors | null => {
    const value = control.value?.toLowerCase() || '';
    return this.bannedWords.some(word => value.includes(word)) ? { bannedWord: true } : null;
  };

  passwordStrengthValidator = (control: AbstractControl): ValidationErrors | null => {
    const value = control.value;
    const hasNumber = /\d/.test(value);
    const hasSymbol = /[!@#$%^&*(),.?":{}|<>]/.test(value);
    return hasNumber && hasSymbol ? null : { weakPassword: true };
  };

  passwordMatchValidator(group: AbstractControl): ValidationErrors | null {
    const pass = group.get('password')?.value;
    const confirm = group.get('confirmPassword')?.value;
    return pass === confirm ? null : { passwordMismatch: true };
  }

  form: FormGroup = this.fb.group({
    username: ['', [Validators.required, this.bannedUsernameValidator]],
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(6), this.passwordStrengthValidator]],
    confirmPassword: ['', Validators.required],
    role: ['', Validators.required]
  }, { validators: this.passwordMatchValidator });

  submit() {
    if (this.form.valid) {
      const { confirmPassword, ...user } = this.form.value;
      this.store.dispatch(addUser({ user: user as User }));
      this.form.reset();
    }
  }
}
