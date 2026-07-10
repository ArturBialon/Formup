import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { toast } from 'ngx-sonner';
import { AccountService } from '../_services/account.service';

export const adminGuard: CanActivateFn = (route, state) => {
  const router = inject(Router);
  const authService = inject(AccountService);

  if (authService.isAdmin()) {
    return true;
  }

  toast.error('UNAUTHORIZED_ERROR');
  router.navigate(['/dashboard']);
  return false;
};