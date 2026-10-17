import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

export interface Pet {
  id: string;
  tutorId: string;
  tutorNome: string;
  nome: string;
  especie: string;
  raca: string;
}

export interface PetFormulario {
  tutorId: string;
  nome: string;
  especie: string;
  raca: string;
}

export interface Historico {
  id: string;
  petId: string;
  data: string;
  descricao: string;
}

export interface HistoricoFormulario {
  data: string;
  descricao: string;
}

@Injectable({ providedIn: 'root' })
export class PetService {
  private readonly http = inject(HttpClient);

  listar() {
    return this.http.get<Pet[]>('/api/pets');
  }

  criar(dados: PetFormulario) {
    return this.http.post<Pet>('/api/pets', dados);
  }

  atualizar(id: string, dados: PetFormulario) {
    return this.http.put<Pet>(`/api/pets/${id}`, dados);
  }

  excluir(id: string) {
    return this.http.delete(`/api/pets/${id}`);
  }

  listarHistorico(petId: string) {
    return this.http.get<Historico[]>(`/api/pets/${petId}/historico`);
  }

  criarHistorico(petId: string, dados: HistoricoFormulario) {
    return this.http.post<Historico>(`/api/pets/${petId}/historico`, dados);
  }
}
