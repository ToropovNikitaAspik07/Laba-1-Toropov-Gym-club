import { Routes } from '@angular/router';
import { Administrator } from './components/administrator/administrator';
import { Manager } from './components/manager/manager';
import { RoleChanger } from './components/role-changer/role-changer';

export const routes: Routes = [
    { path: '', redirectTo: '/role-changer', pathMatch: 'full' },
    { path: 'manager', component: Manager },
    { path: 'administrator', component: Administrator },
    { path: 'role-changer', component: RoleChanger}
];
