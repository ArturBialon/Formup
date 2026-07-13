import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { authGuard } from './_guards/auth.guard';
import { DashboardComponent } from './shared/dashboard/dashboard.component';
import { ManageUsersComponent } from './manage-users/manage-users.component';
import { adminGuard } from './_guards/admin.guard';
import { ServiceContractorsComponent } from './service-contractors/service-contractors.component';
import { ClientsComponent } from './clients/clients.component';
import { WorkCaseListComponent } from './work-cases/work-case-list/work-case-list.component';
import { WorkCaseAddComponent } from './work-cases/work-case-add/work-case-add.component';

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
      { 
        path: 'contractors', 
        component: ServiceContractorsComponent,
        canActivate: [authGuard]
      },
      { 
        path: 'clients', 
        component: ClientsComponent,
        canActivate: [authGuard]
      },
      { 
        path: 'workcases', 
        component: WorkCaseListComponent,
        canActivate: [authGuard]
      },
      { 
        path: 'workcaseadd', 
        component: WorkCaseAddComponent,
        canActivate: [authGuard]
      },
    ],
    canActivate: [authGuard]
  },

  { path: 'errors', loadComponent: () => import('../app/errors/test-errors/test-errors.component').then(m => m.TestErrorsComponent) },
  { path: 'not-found', loadComponent: () => import('../app/errors/not-found/not-found.component').then(m => m.NotFoundComponent) },
  { path: 'server-error', loadComponent: () => import('../app/errors/server-error/server-error.component').then(m => m.ServerErrorComponent) },
  { path: '**', redirectTo: '/not-found', pathMatch: 'full' } 
];