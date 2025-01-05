
import { Routes } from '@angular/router';
import { AuthGuard } from '../../../app/shared/auth.guard';

export const routes: Routes = [
    {
        path: 'google',
        loadComponent: () => import('./google/google.component').then(m => m.GoogleComponent),
        data: {
          title: 'Chiến dịch google',
          requiredPolicy:'Permissions.Campain.View'
        },
        canActivate: [AuthGuard]
    },
    {
        path: 'facebook',
        loadComponent: () => import('./facebook/facebook.component').then(m => m.FacebookComponent),
        data: {
            title: 'Chiến dịch facebook',
            requiredPolicy:'Permissions.Campain.View'
        },
        canActivate: [AuthGuard]
    },
    {
        path: 'youtube',
        loadComponent: () => import('./youtube/youtube.component').then(m => m.YoutubeComponent),
        data: {
            title: 'Chiến dịch youtube',
            requiredPolicy:'Permissions.Campain.View'
        },
        canActivate: [AuthGuard]
    },
    {
        path: 'instagram',
        loadComponent: () => import('./instagram/instagram.component').then(m => m.InstagramComponent),
        data: {
            title: 'Chiến dịch instagram',
            requiredPolicy:'Permissions.Campain.View'
        },
        canActivate: [AuthGuard]
    },
];

