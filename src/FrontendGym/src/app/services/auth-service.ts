import { Injectable, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { tap } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private http = inject(HttpClient);
  private baseUrl = `${environment.apiUrl}/api/auth`;
  
  // Используем Signals для хранения состояния авторизации
  currentUserToken = signal<string | null>(localStorage.getItem('token'));

  login(credentials: any) {
    return this.http.post<string>(`${this.baseUrl}/login`, credentials).pipe(
      tap(token => {
        localStorage.setItem('token', token);
        this.currentUserToken.set(token);
      })
    );
  }
  register(userData: any) {
    return this.http.post(`${this.baseUrl}/register`, userData);
  }
  logout() {
    localStorage.removeItem('token');
    this.currentUserToken.set(null);
  }
}
