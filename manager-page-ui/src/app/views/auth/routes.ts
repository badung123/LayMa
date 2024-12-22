import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '404',
    loadComponent: () => import('./page404/page404.component').then(m => m.Page404Component),
    data: {
      title: 'Lỗi 404'
    }
  },
  {
    path: '403',
    loadComponent: () => import('./page403/page403.component').then(m => m.Page403Component),
    data: {
      title: 'Lỗi 403'
    }
  },
  {
    path: '500',
    loadComponent: () => import('./page500/page500.component').then(m => m.Page500Component),
    data: {
      title: 'Lỗi 500'
    }
  },
  {
    path: 'login',
    loadComponent: () => import('./login/login.component').then(m => m.LoginComponent),
    data: {
      title: 'Trang Đăng nhập'
    }
  },
  {
    path: 'register',
    loadComponent: () => import('./register/register.component').then(m => m.RegisterComponent),
    data: {
      title: 'Trang đăng ký'
    }
  },
  {
    path: 'forgetPassword',
    loadComponent: () => import('./forgetPassword/forgetPassword.component').then(m => m.ForgetPasswordComponent),
    data: {
      title: 'Trang quên mật khẩu'
    }
  },
  {
    path: 'resetPassword',
    loadComponent: () => import('./resetPassword/resetPassword.component').then(m => m.ResetPasswordComponent),
    data: {
      title: 'Trang lấy lại mật khẩu'
    }
  }
];
