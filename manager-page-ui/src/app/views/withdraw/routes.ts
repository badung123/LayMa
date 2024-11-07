
import { Routes } from '@angular/router';

export const routes: Routes = [
    {
        path: '',
        loadComponent: () => import('./withdraw.component').then(m => m.WithDrawComponent),
        data: {
          title: 'Rút tiền'
        }
    }
];

