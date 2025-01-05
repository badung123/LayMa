
import { Routes } from '@angular/router';
import { AuthGuard } from '../../../app/shared/auth.guard';

export const routes: Routes = [
    {
        path: '',
        loadComponent: () => import('./userThongke.component').then(m => m.UserThongkeComponent),
        data: {
          title: 'Thống kê người dùng',
          requiredPolicy:'Permissions.Users.View'
        },
        canActivate: [AuthGuard]
    }
];

