import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { authGuard } from './_guards/auth.guard';
import { DashboardComponent } from './shared/dashboard/dashboard.component';
import { ManageUsersComponent } from './manage-users/manage-users.component';
import { adminGuard } from './_guards/admin.guard';
import { ServiceContractorsComponent } from './service-contractors/service-contractors.component';
import { ClientsComponent } from './clients/client-form/clients.component';
import { WorkCaseListComponent } from './work-cases/work-case-list/work-case-list.component';
import { WorkCaseAddComponent } from './work-cases/work-case-add/work-case-add.component';
import { WorkCaseDetailsComponent } from './work-cases/work-case-details/work-case-details.component';
import { WorkCaseEditComponent } from './work-cases/work-case-edit/work-case-edit.component';

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
        path: 'workcase/add', 
        component: WorkCaseAddComponent,
        canActivate: [authGuard]
      },
      { 
        path: 'workcase/:id', 
        component: WorkCaseDetailsComponent,
        canActivate: [authGuard]
      },
      { path: 'workcase/:id/edit',
        component: WorkCaseEditComponent ,
        canActivate: [authGuard]
      },
    ],
    canActivate: [authGuard]
  },

  { path: 'not-found', loadComponent: () => import('../app/errors/not-found/not-found.component').then(m => m.NotFoundComponent) },
  { path: 'server-error', loadComponent: () => import('../app/errors/server-error/server-error.component').then(m => m.ServerErrorComponent) },
  { path: '**', redirectTo: '/not-found', pathMatch: 'full' } 
];