import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { User } from '../../services/user/user';
import { Auth } from '../../services/auth/auth';

@Component({
  selector: 'app-profile',
  standalone: true,
  templateUrl: './profile.html',
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
})
export class Profile implements OnInit {
  profileForm!: FormGroup;
  selectedFile: File | null = null;
  userId: string = '';
  profilePicPreview: string = '';


  constructor(
    private fb: FormBuilder, 
    private userService: User, 
    private authService: Auth,
    private router: Router
  ) {
    const storedUser = localStorage.getItem('user');
    if (storedUser) {
      const user = JSON.parse(storedUser);
      this.userId = user.id;
    }

    this.profileForm = this.fb.group({
      username: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      role: [''],
      profilePicture: [null]
    });
  }

  ngOnInit() {
    this.userService.getUserById(this.userId).subscribe(user => {
      this.profileForm.patchValue({
        username: user.username,
        email: user.email,
        role: user.role
      });

      // this.profilePicPreview = user.profilePicturePath ? `http://localhost:5025/${user.profilePicturePath}` : '';
      const path = user.profilePicturePath;
      this.profilePicPreview = path
      ? path.startsWith('http') 
        ? path 
        : `http://localhost:5025/${path}`
      : '';
    });
  }

  onFileSelected(event: any) {
    this.selectedFile = event.target.files[0];
    if (this.selectedFile) {
      const reader = new FileReader();
      reader.onload = () => this.profilePicPreview = reader.result as string;
      reader.readAsDataURL(this.selectedFile);
    }
  }

  submit() {
    if (this.profileForm.invalid) return;

    const formData = new FormData();
    formData.append('Username', this.profileForm.value.username);
    formData.append('Email', this.profileForm.value.email);
    formData.append('Role', this.profileForm.value.role);
    if (this.selectedFile) {
      formData.append('ProfilePicture', this.selectedFile);
    }

    // this.userService.updateUser(this.userId, formData).subscribe({
    //   next: () => alert('Profile updated successfully!'),
    //   error: err => alert(err.error?.message || 'Update failed')
    // });
    this.userService.updateUser(this.userId, formData).subscribe({
      next: (updatedUser) => {
        this.authService.updateUserInStorage(updatedUser); // update user details localStorage
        alert('Profile updated successfully!');
      },
      error: err => alert(err.error?.message || 'Update failed')
    });
  }

  // deleteAccount() {
  //   if (!confirm('Are you sure you want to delete your account? This action is irreversible.')) return;

  //   this.userService.deleteUser(this.userId).subscribe({
  //     next: () => {
  //       alert('Account deleted successfully!');
  //       this.authService.logout();
  //     },
  //     error: err => alert(err.error?.message || 'Account deletion failed')
  //   });
  // }
  deleteAccount() {
  if (!confirm('Are you sure you want to delete your account? This action is irreversible.')) return;

  this.userService.deleteUser(this.userId).subscribe({
    next: (res) => {
      alert('Account deleted successfully!');
      this.authService.logout();
    },
    error: err => {
      console.error(err); // helpful for debugging
      alert(err.error?.message || 'Account deletion failed');
    }
  });
}


}
