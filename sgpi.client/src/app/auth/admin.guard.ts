import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from './auth.service';
import { map, take } from 'rxjs/operators';

export const adminGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  // We need to check if user has 'Admin' role.
  // Ideally AuthService should have a method for this or we decode the token.
  // For now, let's assume we can check roles from AuthService or User profile.
  // Since we don't have roles in AuthService yet, we might need to decode token or fetch user profile.

  // Let's decode the token from localStorage for a quick check, or better, add `getRoles()` to AuthService.

  // Assuming AuthService has currentUser$ or similar.
  // Let's implement a simple check based on token payload if possible, or just check if authenticated for now and TODO role check.
  // But the requirement is "Admin Guard".

  // Let's check localStorage token for role claim.
  const token = localStorage.getItem('token');
  if (!token) {
    return router.createUrlTree(['/login']);
  }

  try {
    const payload = JSON.parse(atob(token.split('.')[1]));
    const roles =
      payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] || payload['role'];

    const hasAdminRole = Array.isArray(roles) ? roles.includes('Admin') : roles === 'Admin';

    if (hasAdminRole) {
      return true;
    }
  } catch (e) {
    console.error('Error decoding token', e);
  }

  return router.createUrlTree(['/dashboard']);
};
