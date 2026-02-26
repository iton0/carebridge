import { HttpHandlerFn, HttpEvent, HttpRequest } from '@angular/common/http';
import { inject } from '@angular/core';
import { Observable } from 'rxjs';
import { AuthService } from '../services/auth';

export function loggingInterceptor(
  req: HttpRequest<unknown>,
  next: HttpHandlerFn,
): Observable<HttpEvent<unknown>> {
  const auth = inject(AuthService);
  const authToken = auth.getAuthToken();
  const user = auth.currentUser();

  const authorizedReq = req.clone({
    headers: req.headers.append('X-Authentication-Token', authToken),
  });

  const timestamp = new Date().toISOString();

  // For now, structure the log so it's "Machine Readable."
  console.log(
    `[AUDIT] USER: ${user.id} | ACTION: ${req.method} | PATH: ${req.url} | TIME: ${timestamp}`,
  );

  return next(authorizedReq);
}
