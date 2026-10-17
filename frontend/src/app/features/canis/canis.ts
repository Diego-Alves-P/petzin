import { Component, inject, OnInit, signal, viewChild } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { Canil, CanilFormulario, CanilService } from '../../services/canil.service';
import { lerErro } from '../../services/formato';
import { MensagemErro } from '../../shared/mensagem-erro';
import { CanilForm } from './canil-form';
import { CanilLista } from './canil-lista';

@Component({
  selector: 'app-canis',
  imports: [CanilForm, CanilLista, MensagemErro],
  templateUrl: './canis.html'
})
export class Canis implements OnInit {
  private readonly service = inject(CanilService);
  private readonly form = viewChild.required(CanilForm);

  readonly canis = signal<Canil[]>([]);
  readonly carregando = signal(true);
  readonly erroFormulario = signal('');
  readonly erroLista = signal('');

  ngOnInit() {
    this.carregar();
  }

  carregar() {
    this.service.listar().subscribe({
      next: (dados) => {
        this.canis.set(dados);
        this.carregando.set(false);
      },
      error: (erro: HttpErrorResponse) => {
        this.erroLista.set(lerErro(erro));
        this.carregando.set(false);
      }
    });
  }

  salvar(dados: CanilFormulario) {
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

  editar(canil: Canil) {
    this.erroFormulario.set('');
    this.form().preencher(canil);
  }

  excluir(canil: Canil) {
    if (!confirm('Deseja excluir este canil?')) {
      return;
    }

    this.erroLista.set('');
    this.service.excluir(canil.id).subscribe({
      next: () => this.carregar(),
      error: (erro: HttpErrorResponse) => this.erroLista.set(lerErro(erro))
    });
  }
}
