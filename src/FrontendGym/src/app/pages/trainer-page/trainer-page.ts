import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TrainerService, Trainer } from './../../services/trainer-service';

@Component({
  selector: 'app-trainer-page',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="container">
      <h1>Панель управления тренерами</h1>

      @if (loading()) {
        <p>Загрузка данных...</p>
      } @else {
        <div class="trainer-grid">
          @for (trainer of trainers(); track trainer.id) {
            <div class="trainer-card">
              <h3>{{ trainer.name }}</h3>
              <p>Специализация: {{ trainer.specialization }}</p>
              <button (click)="selectTrainer(trainer.id)">Детали</button>
            </div>
          } @empty {
            <p>Тренеры не найдены.</p>
          }
        </div>
      }

      @if (selectedTrainer()) {
        <div class="details">
          <h3>Выбран: {{ selectedTrainer()?.name }}</h3>
          <!-- Здесь можно добавить форму создания тренировки для этого тренера -->
        </div>
      }
    </div>
  `,
  styles: `
    .trainer-grid { display: grid; grid-template-columns: repeat(3, 1fr); gap: 20px; }
    .trainer-card { padding: 15px; border: 1px solid #aaa; border-radius: 10px; background: #f9f9f9; }
    .container { padding: 20px; }
  `
})
export class TrainerPageComponent implements OnInit {
  private trainerService = inject(TrainerService);

  trainers = signal<Trainer[]>([]);
  selectedTrainer = signal<Trainer | null>(null);
  loading = signal(true);

  ngOnInit() {
    this.loadTrainers();
  }

  loadTrainers() {
    this.trainerService.getTrainers().subscribe({
      next: (data) => {
        this.trainers.set(data);
        this.loading.set(false);
      },
      error: () => this.loading.set(false)
    });
  }

  selectTrainer(id: number) {
    this.trainerService.getTrainerById(id).subscribe(t => this.selectedTrainer.set(t));
  }
}
