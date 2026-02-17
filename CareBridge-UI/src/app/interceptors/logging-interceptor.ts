import { HttpInterceptorFn } from '@angular/common/http';

// TODO: make this HIPAA-compliant access events.
export const loggingInterceptor: HttpInterceptorFn = (req, next) => {
  console.log('[INTERCEPTOR] Request URL:', req.url);
  console.log('[INTERCEPTOR] Request Method:', req.method);

  return next(req);
};
