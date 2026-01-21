import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router'; // Импортируем RouterLink
import { AuthService } from './../../services/auth-service';

@Component({
  selector: 'app-register',
  standalone: true,
  // Добавляем RouterLink в imports, чтобы работала навигация в шаблоне
  imports: [FormsModule, RouterLink], 
  template: `
    <div class="auth-container">
      <h2>Создать аккаунт</h2>
      
      <form (submit)="onRegister()">
        <div class="form-field">
          <input type="email" [(ngModel)]="email" name="email" placeholder="Email" required>
        </div>
        
        <div class="form-field">
          <input type="password" [(ngModel)]="password" name="password" placeholder="Пароль" required>
        </div>

        <button type="submit" [disabled]="isLoading()">
          {{ isLoading() ? 'Регистрация...' : 'Зарегистрироваться' }}
        </button>
      </form>

      @if (error()) {
        <p class="error-message">{{ error() }}</p>
      }

      <!-- Блок с гиперссылкой на логин -->
      <div class="auth-footer">
        <p>Уже зарегистрированы? 
          <a routerLink="/login" class="login-link">Войти в систему</a>
        </p>
      </div>
    </div>
  `,
  styles: [`
    .auth-container {
      max-width: 400px;
      margin: 100px auto;
      padding: 30px;
      border: 1px solid #e0e0e0;
      border-radius: 12px;
      box-shadow: 0 4px 12px rgba(0,0,0,0.05);
      text-align: center;
    }
    .form-field { margin-bottom: 15px; }
    input {
      width: 100%;
      padding: 12px;
      border: 1px solid #ccc;
      border-radius: 6px;
      box-sizing: border-box;
    }
    button {
      width: 100%;
      padding: 12px;
      background-color: #28a745;
      color: white;
      border: none;
      border-radius: 6px;
      cursor: pointer;
      font-weight: bold;
    }
    button:disabled { background-color: #94d3a2; }
    .error-message { color: #dc3545; margin-top: 15px; }
    
    /* Стили для гиперссылки */
    .auth-footer {
      margin-top: 20px;
      padding-top: 15px;
      border-top: 1px solid #eee;
      font-size: 0.9rem;
    }
    .login-link {
      color: #007bff;
      text-decoration: none;
      font-weight: 500;
    }
    .login-link:hover {
      text-decoration: underline;
    }
  `]
})
export class Register {
  private authService = inject(AuthService);
  private router = inject(Router);

  email = '';
  password = '';
  isLoading = signal(false);
  error = signal('');

  onRegister() {
    this.isLoading.set(true);
    this.error.set('');

    const registrationData = { email: this.email, password: this.password };

    this.authService.register(registrationData).subscribe({
      next: () => {
        // После успешной регистрации отправляем на логин
        this.router.navigate(['/login']);
      },
      error: (err) => {
        this.error.set(err.error || 'Ошибка регистрации. Возможно, пользователь уже существует.');
        this.isLoading.set(false);
      }
    });
  }
}
