import { Routes } from '@angular/router';
import { TrainerPageComponent } from './pages/trainer-page/trainer-page';
import { authGuardGuard } from './auth-guard-guard';
import { Register } from './pages/register/register';

export const routes: Routes = [ { 
    path: 'client-manager', 
    loadComponent: () => import('./pages/client-manager/client-manager').then(m => m.ClientManager),
    canActivate: [authGuardGuard] 
  },
  { 
    path: 'login', 
    loadComponent: () => import('./pages/login/login').then(m => m.LoginComponent) 
  },
  { path: 'trainer', component: TrainerPageComponent },
  { path: 'register', component: Register },
  { path: '', redirectTo: '/register', pathMatch: 'full' },
];
