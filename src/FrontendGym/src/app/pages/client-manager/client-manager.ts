import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ClientService } from './../../services/client-service';

@Component({
  selector: 'app-client-manager',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div class="search-box">
      <input [(ngModel)]="cardNumber" placeholder="Введите номер карты">
      <button (click)="findClient()">Найти клиента</button>
    </div>

    <div *ngIf="client" class="client-card">
      <h2>{{ client.name }}</h2>
      <p>Осталось сессий: <strong>{{ client.sessionsLeft }}</strong></p>
      <p>Активен до: {{ client.abonementExpireDate | date }}</p>
      
      <button 
        [disabled]="client.sessionsLeft <= 0" 
        (click)="spendSession()">
        Списать занятие
      </button>
    </div>

    <div *ngIf="error" class="error">{{ error }}</div>
  `,
  styles: [`
    .client-card { border: 1px solid #ccc; padding: 1rem; margin-top: 1rem; border-radius: 8px; }
    .error { color: red; margin-top: 10px; }
  `]
})
export class ClientManager {
  private clientService = inject(ClientService);
  
  cardNumber = '';
  client: any = null;
  error = '';

  findClient() {
    this.error = '';
    this.clientService.getByCard(this.cardNumber).subscribe({
      next: (res) => this.client = res,
      error: (err) => {
        this.error = 'Клиент не найден';
        this.client = null;
      }
    });
  }

  spendSession() {
    this.clientService.useSession(this.cardNumber).subscribe({
      next: () => {
        // Локально обновляем счетчик, чтобы не делать повторный запрос
        if (this.client) this.client.sessionsLeft--;
        alert('Сессия списана');
      },
      error: (err) => alert('Ошибка при списании: ' + err.error?.message || 'Нет сессий')
    });
  }
}
