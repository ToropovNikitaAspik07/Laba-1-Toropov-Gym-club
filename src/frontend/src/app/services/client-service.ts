import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Client } from '../models/client';
import { firstValueFrom, map } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ClientService {
  private apiUrl = `${environment.apiUrl}/Client`;
  constructor(private http: HttpClient) {}
  async getClientByCardNumber(cardNumber: string): Promise<Client> {
    return await firstValueFrom(
      this.http.get<Client>(`${this.apiUrl}/bycard/${cardNumber}`)
    );
  
  }

  addClient(client: Partial<Client>): Promise<Client> {
    return firstValueFrom(this.http.post<Client>(this.apiUrl, client));
  }
}
