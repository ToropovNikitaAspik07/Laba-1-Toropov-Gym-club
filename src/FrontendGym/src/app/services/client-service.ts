import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Client } from '../models/models';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class ClientService {
  private http = inject(HttpClient);
  private baseUrl = `${environment.apiUrl}/api/Client`; // Согласно вашему [Route("[controller]")]

  getByCard(cardNumber: string) {
    return this.http.get<Client>(`${this.baseUrl}/bycard/${cardNumber}`);
  }

  createClient(client: Client) {
    return this.http.post<Client>(this.baseUrl, client);
  }

  useSession(cardNumber: string) {
    return this.http.post(`${this.baseUrl}/use-session/${cardNumber}`, {});
  }

  addSessions(cardNumber: string, count: number) {
    return this.http.post(`${this.baseUrl}/add-sessions`, null, {
      params: { cardNumber, sessionsToAdd: count }
    });
  }
  registerToTraining(data: { cardNumber: string, trainingId: number }) {
  return this.http.post(`${this.baseUrl}/register-to-training`, data);
}
}
