
import { Routes } from '@angular/router';
import { AuthGuard } from '../../shared/auth.guard';

export const routes: Routes = [
    {
        path: '',
        loadComponent: () => import('./agent.component').then(m => m.AgentComponent),
        data: {
          title: 'Đại lý',
          requiredPolicy:'Permissions.Agent.View'
        },
        canActivate: [AuthGuard]
    }
];

