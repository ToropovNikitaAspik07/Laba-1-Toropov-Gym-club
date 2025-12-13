import { Component } from '@angular/core';
import { Client } from '../../models/client';
import { ClientService } from '../../services/client-service';
import { FormsModule } from '@angular/forms';
import { RouterLink } from "@angular/router";

@Component({
  selector: 'app-administrator',
  standalone: true,   
  imports: [FormsModule, RouterLink],
  templateUrl: './administrator.html',
  styleUrl: './administrator.scss',
})
export class Administrator {
  client?: Client | null;
  cardNumber: string  = '';
  constructor(private clientService: ClientService) {}
  
  async getClient() {
    try{
      this.client = await this.clientService
        .getClientByCardNumber(this.cardNumber)
        console.log(this.client);
    }
    catch(error){
      console.error('Error fetching client:', error);
    }
  }
  abonementStatus(): string {
    const currentTime = new Date();
    if(this.client && new Date(this.client.abonementExpireDate) > currentTime) {
      return 'Активен';
    } else {
      return 'Не активен';
    }
  }
    
    
}
