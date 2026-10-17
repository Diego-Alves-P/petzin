import { Component, inject, OnInit, signal, viewChild } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { lerErro } from '../../services/formato';
import { Historico, HistoricoFormulario, Pet, PetFormulario, PetService } from '../../services/pet.service';
import { Tutor, TutorService } from '../../services/tutor.service';
import { MensagemErro } from '../../shared/mensagem-erro';
import { HistoricoPet } from './historico-pet';
import { PetForm } from './pet-form';
import { PetLista } from './pet-lista';

@Component({
  selector: 'app-pets',
  imports: [PetForm, PetLista, HistoricoPet, MensagemErro],
  templateUrl: './pets.html'
})
export class Pets implements OnInit {
  private readonly petsService = inject(PetService);
  private readonly tutoresService = inject(TutorService);
  private readonly form = viewChild.required(PetForm);
  private readonly historicoForm = viewChild(HistoricoPet);

  readonly tutores = signal<Tutor[]>([]);
  readonly pets = signal<Pet[]>([]);
  readonly historico = signal<Historico[]>([]);
  readonly petHistorico = signal<Pet | null>(null);
  readonly carregando = signal(true);
  readonly carregandoHistorico = signal(false);
  readonly erroFormulario = signal('');
  readonly erroLista = signal('');
  readonly erroHistorico = signal('');

  ngOnInit() {
    this.tutoresService.listar().subscribe({
      next: (dados) => this.tutores.set(dados),
      error: (erro: HttpErrorResponse) => this.erroLista.set(lerErro(erro))
    });
    this.carregar();
  }

  carregar() {
    this.petsService.listar().subscribe({
      next: (dados) => {
        this.pets.set(dados);
        this.carregando.set(false);
      },
      error: (erro: HttpErrorResponse) => {
        this.erroLista.set(lerErro(erro));
        this.carregando.set(false);
      }
    });
  }

  salvar(dados: PetFormulario) {
    this.erroFormulario.set('');
    const id = this.form().form.controls.id.value;
    const pedido = id ? this.petsService.atualizar(id, dados) : this.petsService.criar(dados);
    pedido.subscribe({
      next: () => {
        this.form().limpar();
        this.carregar();
      },
      error: (erro: HttpErrorResponse) => this.erroFormulario.set(lerErro(erro))
    });
  }

  editar(pet: Pet) {
    this.erroFormulario.set('');
    this.form().preencher(pet);
  }

  abrirHistorico(pet: Pet) {
    this.petHistorico.set(pet);
    this.erroHistorico.set('');
    this.carregandoHistorico.set(true);
    this.petsService.listarHistorico(pet.id).subscribe({
      next: (dados) => {
        this.historico.set(dados);
        this.carregandoHistorico.set(false);
      },
      error: (erro: HttpErrorResponse) => {
        this.erroHistorico.set(lerErro(erro));
        this.carregandoHistorico.set(false);
      }
    });
  }

  adicionarHistorico(dados: HistoricoFormulario) {
    const pet = this.petHistorico();
    if (!pet) {
      return;
    }

    this.erroHistorico.set('');
    this.petsService.criarHistorico(pet.id, dados).subscribe({
      next: () => {
        this.historicoForm()?.limpar();
        this.abrirHistorico(pet);
      },
      error: (erro: HttpErrorResponse) => this.erroHistorico.set(lerErro(erro))
    });
  }

  excluir(pet: Pet) {
    if (!confirm('Deseja excluir este pet?')) {
      return;
    }

    this.erroLista.set('');
    this.petsService.excluir(pet.id).subscribe({
      next: () => {
        if (this.petHistorico()?.id === pet.id) {
          this.petHistorico.set(null);
          this.historico.set([]);
        }
        this.carregar();
      },
      error: (erro: HttpErrorResponse) => this.erroLista.set(lerErro(erro))
    });
  }
}
