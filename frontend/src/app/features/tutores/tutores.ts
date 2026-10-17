import { Component, inject, OnInit, signal, viewChild } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { lerErro } from '../../services/formato';
import { Tutor, TutorFormulario, TutorService } from '../../services/tutor.service';
import { MensagemErro } from '../../shared/mensagem-erro';
import { TutorForm } from './tutor-form';
import { TutorLista } from './tutor-lista';

@Component({
  selector: 'app-tutores',
  imports: [TutorForm, TutorLista, MensagemErro],
  templateUrl: './tutores.html'
})
export class Tutores implements OnInit {
  private readonly service = inject(TutorService);
  private readonly form = viewChild.required(TutorForm);

  readonly tutores = signal<Tutor[]>([]);
  readonly carregando = signal(true);
  readonly erroFormulario = signal('');
  readonly erroLista = signal('');

  ngOnInit() {
    this.carregar();
  }

  carregar() {
    this.service.listar().subscribe({
      next: (dados) => {
        this.tutores.set(dados);
        this.carregando.set(false);
      },
      error: (erro: HttpErrorResponse) => {
        this.erroLista.set(lerErro(erro));
        this.carregando.set(false);
      }
    });
  }

  salvar(dados: TutorFormulario) {
    this.erroFormulario.set('');
    const id = this.form().form.controls.id.value;
    const pedido = id ? this.service.atualizar(id, dados) : this.service.criar(dados);
    pedido.subscribe({
      next: () => {
        this.form().limpar();
        this.carregar();
      },
      error: (erro: HttpErrorResponse) => this.erroFormulario.set(lerErro(erro))
    });
  }

  editar(tutor: Tutor) {
    this.erroFormulario.set('');
    this.form().preencher(tutor);
  }

  excluir(tutor: Tutor) {
    if (!confirm('Deseja excluir este tutor?')) {
      return;
    }

    this.erroLista.set('');
    this.service.excluir(tutor.id).subscribe({
      next: () => this.carregar(),
      error: (erro: HttpErrorResponse) => this.erroLista.set(lerErro(erro))
    });
  }
}
