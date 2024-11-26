import { Routes } from '@angular/router';
import { AuthGuard } from 'src/app/shared/auth.guard';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () => import('./dashboard.component').then(m => m.DashboardComponent),
    data: {
      title: 'Trang Quản Lý',
      requiredPolicy:'Permissions.Dashboard.View'
    },
    canActivate: [AuthGuard]
  }
];

