import { Component, inject, input, output } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MensagemErro } from '../../shared/mensagem-erro';
import { Canil, CanilFormulario } from '../../services/canil.service';

@Component({
  selector: 'app-canil-form',
  imports: [ReactiveFormsModule, MensagemErro],
  templateUrl: './canil-form.html'
})
export class CanilForm {
  private readonly fb = inject(FormBuilder);

  readonly erro = input('');
  readonly salvarFormulario = output<CanilFormulario>();
  readonly portes = ['Pequeno', 'Médio', 'Grande'];

  readonly form = this.fb.nonNullable.group({
    id: [''],
    numero: [1, [Validators.required, Validators.min(1)]],
    porte: ['', Validators.required]
  });

  editando(): boolean {
    return this.form.controls.id.value !== '';
  }

  preencher(canil: Canil) {
    this.form.setValue({
      id: canil.id,
      numero: canil.numero,
      porte: canil.porte
    });
  }

  limpar() {
    this.form.reset({ id: '', numero: 1, porte: '' });
  }

  enviar() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const valor = this.form.getRawValue();
    this.salvarFormulario.emit({
      numero: Number(valor.numero),
      porte: valor.porte
    });
  }
}
