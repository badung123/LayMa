
import { Routes } from '@angular/router';
import { AuthGuard } from 'src/app/shared/auth.guard';

export const routes: Routes = [
    {
        path: 'create',
        loadComponent: () => import('./create/shortlink.component').then(m => m.ShortLinkComponent),
        data: {
          title: 'Tạo Mới Link Rút Gọn',
          requiredPolicy:'Permissions.ShortLink.Create'
        },
        canActivate: [AuthGuard]
    },
    {
        path: 'list',
        loadComponent: () => import('./list/listshortlink.component').then(m => m.ListShortLinkComponent),
        data: {
            title: 'Trang Quản Lý Link Rút Gọn',
            requiredPolicy:'Permissions.ShortLink.View'
        },
        canActivate: [AuthGuard]
    },
];

