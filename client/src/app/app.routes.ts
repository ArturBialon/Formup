import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { authGuard } from './_guards/auth.guard'; // Funkcyjny guard

export const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'home', redirectTo: '', pathMatch: 'full' },
  
  {
    path: 'dashboard',
    canActivate: [authGuard],
    loadComponent: () => import('./shared/dashboard/dashboard.component').then(m => m.DashboardComponent),
    children: [
      // { path: 'cases', loadComponent: () => import('./cases/case.component').then(m => m.CaseComponent) }
    ]
  },
  
  { path: 'errors', loadComponent: () => import('./errors/test-errors/test-errors.component').then(m => m.TestErrorsComponent) },
  { path: 'not-found', loadComponent: () => import('./errors/not-found/not-found.component').then(m => m.NotFoundComponent) },
  { path: 'server-error', loadComponent: () => import('./errors/server-error/server-error.component').then(m => m.ServerErrorComponent) },
  { path: '**', redirectTo: 'not-found', pathMatch: 'full' }
];