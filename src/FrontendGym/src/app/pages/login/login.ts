import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AuthService } from './../../services/auth-service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule],
  template: `
    <div class="login-container">
      <h2>Вход в систему</h2>
      <form (submit)="onLogin()">
        <input type="email" [(ngModel)]="email" name="email" placeholder="Email" required>
        <input type="password" [(ngModel)]="password" name="password" placeholder="Пароль" required>
        
        <button type="submit" [disabled]="loading()">
          {{ loading() ? 'Вход...' : 'Войти' }}
        </button>
      </form>

      @if (error()) {
        <p class="error">{{ error() }}</p>
      }
    </div>
  `,
  styles: `
    .login-container { max-width: 300px; margin: 50px auto; padding: 20px; border: 1px solid #ddd; border-radius: 8px; }
    input { display: block; width: 100%; margin-bottom: 10px; padding: 8px; }
    .error { color: red; font-size: 0.9rem; }
  `
})
export class LoginComponent {
  private authService = inject(AuthService);
  private router = inject(Router);

  email = '';
  password = '';
  loading = signal(false);
  error = signal('');

  onLogin() {
    this.loading.set(true);
    this.error.set('');

    this.authService.login({ email: this.email, password: this.password }).subscribe({
      next: () => {
        this.router.navigate(['/trainer']);
      },
      error: (err) => {
        this.error.set(err.error || 'Ошибка авторизации');
        this.loading.set(false);
      }
    });
  }
}
