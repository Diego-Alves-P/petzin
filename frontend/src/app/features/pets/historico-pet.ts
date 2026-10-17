import { Component, inject, input, output } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { formatarData } from '../../services/formato';
import { Historico, HistoricoFormulario } from '../../services/pet.service';
import { MensagemErro } from '../../shared/mensagem-erro';

@Component({
  selector: 'app-historico-pet',
  imports: [ReactiveFormsModule, MensagemErro],
  templateUrl: './historico-pet.html'
})
export class HistoricoPet {
  private readonly fb = inject(FormBuilder);

  readonly nomePet = input('');
  readonly itens = input<Historico[]>([]);
  readonly carregando = input(false);
  readonly erro = input('');
  readonly salvarHistorico = output<HistoricoFormulario>();
  readonly formatarData = formatarData;

  readonly form = this.fb.nonNullable.group({
    data: ['', Validators.required],
    descricao: ['', Validators.required]
  });

  enviar() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const valor = this.form.getRawValue();
    this.salvarHistorico.emit({
      data: valor.data,
      descricao: valor.descricao.trim()
    });
  }

  limpar() {
    this.form.reset({ data: '', descricao: '' });
  }
}
