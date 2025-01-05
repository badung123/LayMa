
import { Routes } from '@angular/router';
import { AuthGuard } from '../../shared/auth.guard';

export const routes: Routes = [
    {
        path: 'quicklink',
        loadComponent: () => import('./quicklink/quicklink.component').then(m => m.QuickLinkComponent),
        data: {
          title: 'Tạo nhanh link rút gọn',
          requiredPolicy:'Permissions.ToolAPI.View'
        },
        canActivate: [AuthGuard]
    },
    {
        path: 'deverloperAPI',
        loadComponent: () => import('./deverloperAPI/deverloperAPI.component').then(m => m.DeverloperAPIComponent),
        data: {
            title: 'Trang quản lý api cho các nhà phát triển',
            requiredPolicy:'Permissions.ToolAPI.View'
        },
        canActivate: [AuthGuard]
    },
];

