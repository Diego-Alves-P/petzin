import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

export interface Reserva {
  id: string;
  petId: string;
  petNome: string;
  tutorNome: string;
  canilId: string;
  canilNumero: number;
  dataEntrada: string;
  dataSaida: string;
  valorDiaria: number;
  valorTotal: number;
  status: string;
}

export interface ReservaFormulario {
  petId: string;
  canilId: string;
  dataEntrada: string;
  dataSaida: string;
  valorDiaria: number;
  status: string;
}

@Injectable({ providedIn: 'root' })
export class ReservaService {
  private readonly http = inject(HttpClient);

  listar() {
    return this.http.get<Reserva[]>('/api/reservas');
  }

  criar(dados: ReservaFormulario) {
    return this.http.post<Reserva>('/api/reservas', dados);
  }

  atualizar(id: string, dados: ReservaFormulario) {
    return this.http.put<Reserva>(`/api/reservas/${id}`, dados);
  }

  excluir(id: string) {
    return this.http.delete(`/api/reservas/${id}`);
  }
}
