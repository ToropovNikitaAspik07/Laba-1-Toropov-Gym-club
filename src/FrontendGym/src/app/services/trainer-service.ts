import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment.development';

export interface Trainer {
  id: number;
  name: string;
  specialization: string;
}

@Injectable({ providedIn: 'root' })
export class TrainerService {
  private http = inject(HttpClient);
  private baseUrl = `${environment.apiUrl}/api/Trainer`;

  // Получить список всех тренеров
  getTrainers(): Observable<Trainer[]> {
    return this.http.get<Trainer[]>(this.baseUrl);
  }

  getTrainerById(id: number): Observable<Trainer> {
    return this.http.get<Trainer>(`${this.baseUrl}/${id}`);
  }
  createTraining(trainingData: any): Observable<any> {
    return this.http.post(`${this.baseUrl}/create-training`, trainingData);
  }
}
