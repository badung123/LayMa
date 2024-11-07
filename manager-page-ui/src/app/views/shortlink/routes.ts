
import { Routes } from '@angular/router';

export const routes: Routes = [
    {
        path: 'create',
        loadComponent: () => import('./create/shortlink.component').then(m => m.ShortLinkComponent),
        data: {
          title: 'Tạo Mới Link Rút Gọn'
        }
    },
    {
        path: 'list',
        loadComponent: () => import('./list/listshortlink.component').then(m => m.ListShortLinkComponent),
        data: {
        title: 'Trang Quản Lý Link Rút Gọn'
        }
    },
];

