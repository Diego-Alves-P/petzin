import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

export interface Canil {
  id: string;
  numero: number;
  porte: string;
}

export interface CanilFormulario {
  numero: number;
  porte: string;
}

@Injectable({ providedIn: 'root' })
export class CanilService {
  private readonly http = inject(HttpClient);

  listar() {
    return this.http.get<Canil[]>('/api/canis');
  }

  criar(dados: CanilFormulario) {
    return this.http.post<Canil>('/api/canis', dados);
  }

  atualizar(id: string, dados: CanilFormulario) {
    return this.http.put<Canil>(`/api/canis/${id}`, dados);
  }

  excluir(id: string) {
    return this.http.delete(`/api/canis/${id}`);
  }
}
