// import { Component, signal, computed, effect } from '@angular/core';
// import { CommonModule } from '@angular/common';
// import { ChartConfiguration, ChartType } from 'chart.js';
// import { NgChartsModule } from 'ng2-charts';
// import { UserService } from '../../services/user';
// import { FormsModule } from '@angular/forms';

// @Component({
//   selector: 'app-user-dashboard',
//   standalone: true,
//   imports: [CommonModule, FormsModule, NgChartsModule],
//   templateUrl: './user-dashboard.html',
// })
// export class UserDashboard {
//   searchTerm = signal('');
//   users = signal<any[]>([]);
//   selectedGender = signal('');
//   filteredUsers = computed(() =>
//   this.users().filter(user => {
//     const term = this.searchTerm().toLowerCase();
//     const gender = this.selectedGender();

//     return (
//       (!gender || user.gender === gender) &&
//       (
//         user.firstName.toLowerCase().includes(term) ||
//         user.lastName.toLowerCase().includes(term) ||
//         user.gender.toLowerCase().includes(term) ||
//         (user.role?.toLowerCase() || '').includes(term) ||
//         (user.address?.state?.toLowerCase() || '').includes(term)
//       )
//     );
//   })
// );



//   pieChartLabels = ['Male', 'Female'];
//   pieChartData = { labels: this.pieChartLabels, datasets: [{ data: [0, 0] }] };
//   pieChartType: ChartType = 'pie';

//   constructor(private userService: UserService) {
//     this.userService.getUsers().subscribe((res: any) => {
//       this.users.set(res.users);
//       const male = res.users.filter((u: any) => u.gender === 'male').length;
//       const female = res.users.filter((u: any) => u.gender === 'female').length;
//       this.pieChartData.datasets[0].data = [male, female];
//     });
//   }
// }

import { Component, signal, computed, Signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgChartsModule } from 'ng2-charts';
import { UserService } from '../../services/user';
import { WritableSignal } from '@angular/core';
import { ChartType } from 'chart.js';

@Component({
  selector: 'app-user-dashboard',
  standalone: true,
  imports: [CommonModule, FormsModule, NgChartsModule],
  templateUrl: './user-dashboard.html',
  styleUrl: './user-dashboard.css'
})
export class UserDashboard {
  users = signal<any[]>([]);
  searchTerm = signal('');


  selectedGenders = signal<Set<string>>(new Set());
  selectedRoles = signal<Set<string>>(new Set());


  availableGenders = ['male', 'female'];
  availableRoles = ['admin', 'moderator', 'user'];


  filteredUsers = computed(() =>
    this.users().filter(user => {
      const genderMatch =
        this.selectedGenders().size === 0 ||
        this.selectedGenders().has(user.gender);

      const roleMatch =
        this.selectedRoles().size === 0 ||
        this.selectedRoles().has(user.role);

      const term = this.searchTerm().toLowerCase();
      const termMatch =
        user.firstName.toLowerCase().includes(term) ||
        user.lastName.toLowerCase().includes(term) ||
        user.gender.toLowerCase().includes(term) ||
        (user.role?.toLowerCase() || '').includes(term) ||
        (user.address?.state?.toLowerCase() || '').includes(term);

      return genderMatch && roleMatch && termMatch;
    })
  );


  pieChartLabels = ['Male', 'Female'];
  pieChartData = { labels: this.pieChartLabels, datasets: [{ data: [0, 0] }] };
  pieChartType: ChartType = 'pie';

  constructor(private userService: UserService) {
    this.userService.getUsers().subscribe((res: any) => {
      this.users.set(res.users);
      const male = res.users.filter((u: any) => u.gender === 'male').length;
      const female = res.users.filter((u: any) => u.gender === 'female').length;
      this.pieChartData.datasets[0].data = [male, female];
    });
  }

  toggleSelection(setSignal: WritableSignal<Set<string>>, value: string) {
    const current = new Set(setSignal());
    if (current.has(value)) current.delete(value);
    else current.add(value);
    setSignal.set(current);
  }

}
