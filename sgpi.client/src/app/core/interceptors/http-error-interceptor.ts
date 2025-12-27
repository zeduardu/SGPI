import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { MessageService } from '@services/message.service';
import { catchError, throwError } from 'rxjs';

export const httpErrorInterceptor: HttpInterceptorFn = (req, next) => {
  const router = inject(Router);
  const notificationService = inject(MessageService);
  return next(req).pipe(
    catchError(
      (error: HttpErrorResponse) => {
        // Log do erro
        // console.error('HTTP Error:', error);
        // Tratamento especifico por status
        switch (error.status) {
          case 401:
            // Token expirado ou invalido
            localStorage.removeItem('token');
            router.navigate(['/login']);
            notificationService.showError('Sessão expirada. Faça login novamente.');
            break;

          case 403:
            notificationService.showError('Você não tem permissão para esta ação.');
            break;

          case 404:
            notificationService.showError('Recurso não encontrado.');
            break;

          case 500:
            notificationService.showError('Erro interno do servidor. Tente novamente mais tarde.');
            break;

          default:
            if (error.error?.message) {
              notificationService.showError(error.error.message);
            } else {
              notificationService.showError('Ocorreu um erro inesperado.')
            }
        }
        return throwError(() => error);
      }
    ),
  );
};
