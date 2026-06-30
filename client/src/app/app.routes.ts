import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { authGuard } from './_guards/auth.guard'; // Funkcyjny guard
import { DashboardComponent } from './shared/dashboard/dashboard.component';

export const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: '', redirectTo: '', pathMatch: 'full' },
  
  {
    path: 'dashboard',
    component: DashboardComponent,
    canActivate: [authGuard]
  },
  
  { path: 'errors', loadComponent: () => import('./errors/test-errors/test-errors.component').then(m => m.TestErrorsComponent) },
  { path: 'not-found', loadComponent: () => import('./errors/not-found/not-found.component').then(m => m.NotFoundComponent) },
  { path: 'server-error', loadComponent: () => import('./errors/server-error/server-error.component').then(m => m.ServerErrorComponent) },
  { path: '**', redirectTo: 'not-found', pathMatch: 'full' }
];