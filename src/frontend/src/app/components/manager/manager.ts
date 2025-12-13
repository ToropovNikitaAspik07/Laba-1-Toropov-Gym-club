import { Component } from '@angular/core';
import { Client } from '../../models/client';
import { ClientService } from '../../services/client-service';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-manager',
  imports: [FormsModule,RouterLink],
  templateUrl: './manager.html',
  styleUrl: './manager.scss',
})
export class Manager {
  client?: Client;
  name: string = '';
  abonementExpireDate: Date = new Date();
  sessionsLeft: number = 0;
 
  constructor(private clientService: ClientService) {}
  
 
  createClient() {
   // console.log('Creating client:', this.name, this.abonementExpireDate, this.sessionsLeft);
    const DateStr = this.abonementExpireDate.toString();
    this.clientService.addClient({ name: this.name, abonementExpireDate: DateStr, sessionsLeft: this.sessionsLeft })
      .then(c => this.client = c)
      .catch(err => console.error(err));
  }
}
