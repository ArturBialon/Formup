import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { authGuard } from './_guards/auth.guard';
import { DashboardComponent } from './shared/dashboard/dashboard.component';
import { ManageUsersComponent } from './manage-users/manage-users.component';
import { adminGuard } from './_guards/admin.guard';

export const routes: Routes = [
  { path: 'home', component: HomeComponent },
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  
  {
    path: 'dashboard',
    component: DashboardComponent,
    children: [
      { 
        path: 'manage-users', 
        component: ManageUsersComponent,
        canActivate: [adminGuard]
      },
    ],
    canActivate: [authGuard]
  },

  { path: 'errors', loadComponent: () => import('../app/errors/test-errors/test-errors.component').then(m => m.TestErrorsComponent) },
  { path: 'not-found', loadComponent: () => import('../app/errors/not-found/not-found.component').then(m => m.NotFoundComponent) },
  { path: 'server-error', loadComponent: () => import('../app/errors/server-error/server-error.component').then(m => m.ServerErrorComponent) },
  { path: '**', redirectTo: '/not-found', pathMatch: 'full' } 
];