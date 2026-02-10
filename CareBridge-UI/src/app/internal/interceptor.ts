import { HttpHandlerFn, HttpEvent, HttpRequest } from '@angular/common/http';
import { Observable } from 'rxjs';

// TODO: make this HIPAA-compliant access events.
export function loggingInterceptor(
  req: HttpRequest<unknown>,
  next: HttpHandlerFn,
): Observable<HttpEvent<unknown>> {
  console.log('[INTERCEPTOR]', req.url);
  return next(req);
}
