import { HttpHandlerFn, HttpEvent, HttpRequest } from '@angular/common/http';
import { Observable } from 'rxjs';
import { inject } from '@angular/core';
import { AuthService } from '../services/auth';

export function loggingInterceptor(
  req: HttpRequest<unknown>,
  next: HttpHandlerFn,
): Observable<HttpEvent<unknown>> {
  // 1. Inject the AuthService
  const authService = inject(AuthService);

  // 2. Get the user (e.g., from a 'user' property or localStorage)
  // If your AuthService has a 'currentUser' property:
  const user = authService.getUsername() || 'Anonymous';

  const timestamp = new Date().toISOString();

  console.log(
    `[AUDIT] USER: ${user} | ACTION: ${req.method} | PATH: ${req.url} | TIME: ${timestamp}`,
  );

  return next(req);
}
