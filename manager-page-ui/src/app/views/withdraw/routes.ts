
import { Routes } from '@angular/router';
import { AuthGuard } from '../../shared/auth.guard';

export const routes: Routes = [
    {
        path: '',
        loadComponent: () => import('./withdraw.component').then(m => m.WithDrawComponent),
        data: {
          title: 'Rút tiền',
          requiredPolicy:'Permissions.WithDraw.Create'
        },
        canActivate: [AuthGuard]
    }
];

