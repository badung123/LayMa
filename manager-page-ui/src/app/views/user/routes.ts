
import { Routes } from '@angular/router';
import { AuthGuard } from '../../../app/shared/auth.guard';

export const routes: Routes = [
    {
        path: '',
        loadComponent: () => import('./user.component').then(m => m.UserComponent),
        data: {
          title: 'Quản lý người dùng',
          requiredPolicy:'Permissions.Users.View'
        },
        canActivate: [AuthGuard]
    }
];

