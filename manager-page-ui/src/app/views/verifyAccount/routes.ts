
import { Routes } from '@angular/router';
import { AuthGuard } from '../../../app/shared/auth.guard';

export const routes: Routes = [
    {
        path: '',
        loadComponent: () => import('./verifyAccount.component').then(m => m.VerifyAccountComponent),
        data: {
          title: 'Xác minh tài khoản',
          requiredPolicy:'Permissions.VerifyAccount.Create'
        },
        canActivate: [AuthGuard]
    }
];

