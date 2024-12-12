
import { Routes } from '@angular/router';
import { AuthGuard } from '../../../app/shared/auth.guard';

export const routes: Routes = [
    {
        path: '',
        loadComponent: () => import('./withdrawmanager.component').then(m => m.WithDrawManagerComponent),
        data: {
          title: 'Quản lý rút tiền',
          requiredPolicy:'Permissions.AdminWithDraw.View'
        },
        canActivate: [AuthGuard]
    }
];

