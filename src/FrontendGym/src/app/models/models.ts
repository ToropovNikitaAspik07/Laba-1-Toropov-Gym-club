export interface AuthResponse {
  token: string;
}

export interface Client {
  name: string;
  abonementExpireDate: Date;
  cardNumber: string;
  sessionsLeft: number;
}

export interface TrainingRegistration {
  cardNumber: string;
  trainingId: number;
}

export interface StatItem {
  date: string;
  count: number;
}