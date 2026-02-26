import { Injectable, signal } from '@angular/core';

export interface User {
  id: string;
  name: string;
  role: 'clinician' | 'admin';
}

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  readonly currentUser = signal<User>({
    id: 'USR-123',
    name: 'Dr. Tonuzi',
    role: 'clinician',
  });

  getAuthToken(): string {
    // In a real app, might pull this from localStorage or a cookie.
    return 'mpath-debug-token-2026';
  }

  logout() {
    console.log('--> User logged out, clearing state.');
    // Logic to redirect or clear tokens would go here
  }
}
