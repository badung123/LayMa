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
        path: 'tool',
        loadChildren: () => import('./views/tool/routes').then((m) => m.routes)
      },
      {
        path: 'withdraw',
        loadChildren: () => import('./views/withdraw/routes').then((m) => m.routes)
      },
      {
        path: 'campain',
        loadChildren: () => import('./views/campain/routes').then((m) => m.routes)
      },
      {
        path: 'user',
        loadChildren: () => import('./views/user/routes').then((m) => m.routes)
      },
      {
        path: 'userThongke',
        loadChildren: () => import('./views/userThongke/routes').then((m) => m.routes)
      },
      {
        path: 'withdrawmanager',
        loadChildren: () => import('./views/withdrawmanager/routes').then((m) => m.routes)
      },
      {
        path: 'agent',
        loadChildren: () => import('./views/agent/routes').then((m) => m.routes)
      },
      {
        path: 'verifyAccount',
        loadChildren: () => import('./views/verifyAccount/routes').then((m) => m.routes)
      },
      {
        path: 'logs',
        loadChildren: () => import('./views/log/routes').then((m) => m.routes)
      },
    ]
  },
  { path: '**', redirectTo: 'dashboard' }
];
