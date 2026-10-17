import { Component, inject, OnInit, signal, viewChild } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { Canil, CanilService } from '../../services/canil.service';
import { lerErro } from '../../services/formato';
import { Pet, PetService } from '../../services/pet.service';
import { Reserva, ReservaFormulario, ReservaService } from '../../services/reserva.service';
import { MensagemErro } from '../../shared/mensagem-erro';
import { ReservaForm } from './reserva-form';
import { ReservaLista } from './reserva-lista';

@Component({
  selector: 'app-reservas',
  imports: [ReservaForm, ReservaLista, MensagemErro],
  templateUrl: './reservas.html'
})
export class Reservas implements OnInit {
  private readonly reservasService = inject(ReservaService);
  private readonly petsService = inject(PetService);
  private readonly canisService = inject(CanilService);
  private readonly form = viewChild.required(ReservaForm);

  readonly reservas = signal<Reserva[]>([]);
  readonly pets = signal<Pet[]>([]);
  readonly canis = signal<Canil[]>([]);
  readonly carregando = signal(true);
  readonly erroFormulario = signal('');
  readonly erroLista = signal('');

  ngOnInit() {
    this.petsService.listar().subscribe({
      next: (dados) => this.pets.set(dados),
      error: (erro: HttpErrorResponse) => this.erroLista.set(lerErro(erro))
    });
    this.canisService.listar().subscribe({
      next: (dados) => this.canis.set(dados),
      error: (erro: HttpErrorResponse) => this.erroLista.set(lerErro(erro))
    });
    this.carregar();
  }

  carregar() {
    this.reservasService.listar().subscribe({
      next: (dados) => {
        this.reservas.set(dados);
        this.carregando.set(false);
      },
      error: (erro: HttpErrorResponse) => {
        this.erroLista.set(lerErro(erro));
        this.carregando.set(false);
      }
    });
  }

  salvar(dados: ReservaFormulario) {
    this.erroFormulario.set('');
    const id = this.form().form.controls.id.value;
    const pedido = id ? this.reservasService.atualizar(id, dados) : this.reservasService.criar(dados);
    pedido.subscribe({
      next: () => {
        this.form().limpar();
        this.carregar();
      },
      error: (erro: HttpErrorResponse) => this.erroFormulario.set(lerErro(erro))
    });
  }

  editar(reserva: Reserva) {
    this.erroFormulario.set('');
    this.form().preencher(reserva);
  }

  excluir(reserva: Reserva) {
    if (!confirm('Deseja excluir esta reserva?')) {
      return;
    }

    this.erroLista.set('');
    this.reservasService.excluir(reserva.id).subscribe({
      next: () => this.carregar(),
      error: (erro: HttpErrorResponse) => this.erroLista.set(lerErro(erro))
    });
  }
}
