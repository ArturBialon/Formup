import { inject } from '@angular/core';
import { CanActivateFn, Router, UrlTree } from '@angular/router';
import { AccountService } from '../_services/account.service';
import { filter, map, take } from 'rxjs/operators';

export const authGuard: CanActivateFn = () => {
  const accountService = inject(AccountService);
  const router = inject(Router);

  return accountService.user$.pipe(
    filter((user) => user !== undefined),
    take(1),
    map((user) => {
      if (user) return true;
      return router.createUrlTree(['/home']);
    })
  );
};