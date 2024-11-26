import { Routes } from '@angular/router';
import { DefaultLayoutComponent } from './layout';

export const routes: Routes = [
  
  {
    path: 'auth',
    loadChildren: () =>
      import('./views/auth/routes').then((m) => m.routes),
  },
  
  {
    path: '',
    component: DefaultLayoutComponent,
    data: {
      title: 'Home'
    },
    children: [
 
      {
        path: 'dashboard',
        loadChildren: () => import('./views/dashboard/routes').then((m) => m.routes)
      },
      {
        path: 'shortlink',
        loadChildren: () => import('./views/shortlink/routes').then((m) => m.routes)
      },
      {
        path: 'withdraw',
        loadChildren: () => import('./views/withdraw/routes').then((m) => m.routes)
      },
    ]
  },
  { path: '**', redirectTo: 'dashboard' }
];
