import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';

export const authGuardGuard: CanActivateFn = (route, state) => {
   const router = inject(Router);
  
  const token = localStorage.getItem('token');

  if (token) {
    return true;
  }
  // Если токена нет, перенаправляем на страницу логина
  // Сохраняем url, на который пользователь хотел попасть, чтобы вернуться после входа
  router.navigate(['/login'], { queryParams: { returnUrl: state.url } });
  return false;
};
