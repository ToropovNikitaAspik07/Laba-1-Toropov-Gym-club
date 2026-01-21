import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment.development';

@Injectable({ providedIn: 'root' })
export class StatsService {
  private http = inject(HttpClient);
  private baseUrl = `${environment.apiUrl}/api/Stats`;

  // Метод для получения статистики за конкретный день
  getDayStats(date: Date): Observable<any> {
    const dateStr = date.toISOString();
    return this.http.get(`${this.baseUrl}/day`, { params: { date: dateStr } });
  }

  getWeekStats(): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/week`);
  }

  getMonthStats(): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/month`);
  }
}
