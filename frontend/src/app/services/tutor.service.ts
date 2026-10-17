import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

export interface Tutor {
  id: string;
  nome: string;
  telefone: string;
  email: string | null;
}

export interface TutorFormulario {
  nome: string;
  telefone: string;
  email: string | null;
}

@Injectable({ providedIn: 'root' })
export class TutorService {
  private readonly http = inject(HttpClient);

  listar() {
    return this.http.get<Tutor[]>('/api/tutores');
  }

  criar(dados: TutorFormulario) {
    return this.http.post<Tutor>('/api/tutores', dados);
  }

  atualizar(id: string, dados: TutorFormulario) {
    return this.http.put<Tutor>(`/api/tutores/${id}`, dados);
  }

  excluir(id: string) {
    return this.http.delete(`/api/tutores/${id}`);
  }
}
