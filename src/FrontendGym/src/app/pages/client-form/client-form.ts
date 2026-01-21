import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ClientService } from './../../services/client-service';
import { Client } from '../../models/models';

@Component({
  selector: 'app-client-form',
  standalone: true,
  imports: [FormsModule],
  template: `
    <div class="card">
      <h3>Регистрация клиента</h3>
      <input [(ngModel)]="newClient().name" placeholder="ФИО">
      <input [(ngModel)]="newClient().cardNumber" placeholder="Номер карты">
      <input type="number" [(ngModel)]="newClient().sessionsLeft" placeholder="Кол-во занятий">
      <input type="date" (change)="setDate($event)" placeholder="Срок действия">
      
      <button (click)="save()">Сохранить</button>
      
      @if (message()) {
        <p>{{ message() }}</p>
      }
    </div>
  `
})
export class ClientFormComponent {
  private clientService = inject(ClientService);
  
  newClient = signal<Partial<Client>>({ sessionsLeft: 0 });
  message = signal('');

  setDate(event: any) {
    this.newClient.update(c => ({ ...c, abonementExpireDate: new Date(event.target.value) }));
  }

  save() {
    this.clientService.createClient(this.newClient() as Client).subscribe({
      next: () => this.message.set('Клиент успешно создан!'),
      error: (err) => this.message.set('Ошибка: ' + err.error)
    });
  }
}
