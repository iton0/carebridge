import { inject, Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { tap } from 'rxjs/operators';
import { jwtDecode } from 'jwt-decode';
import { Router } from '@angular/router';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private apiUrl = 'http://localhost:5138/api/auth';
  private router = inject(Router);

  currentUser = signal<{ name: string; role: string } | null>(this.decodeUser());

  constructor(private http: HttpClient) {}

  login(credentials: any) {
    return this.http.post<{ token: string }>(`${this.apiUrl}/login`, credentials).pipe(
      tap((response) => {
        localStorage.setItem('carebridge_token', response.token);
        this.currentUser.set(this.decodeUser());
      }),
    );
  }

  logout() {
    localStorage.removeItem('carebridge_token');
    this.currentUser.set(null);
    this.router.navigate(['/login']);
  }

  getToken(): string | null {
    return localStorage.getItem('carebridge_token');
  }

  getUsername(): string | null {
    const user = this.currentUser();
    return user ? user.name : 'Anonymous';
  }

  private decodeUser() {
    const token = this.getToken();
    if (!token) return null;
    try {
      const decoded: any = jwtDecode(token);
      return {
        name: decoded.unique_name || decoded.name || 'User',
        role: decoded.role || 'User',
      };
    } catch {
      return null;
    }
  }
}
