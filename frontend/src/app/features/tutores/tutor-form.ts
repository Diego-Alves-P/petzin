import { Component, inject, input, output } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MensagemErro } from '../../shared/mensagem-erro';
import { Tutor, TutorFormulario } from '../../services/tutor.service';

@Component({
  selector: 'app-tutor-form',
  imports: [ReactiveFormsModule, MensagemErro],
  templateUrl: './tutor-form.html'
})
export class TutorForm {
  private readonly fb = inject(FormBuilder);

  readonly erro = input('');
  readonly salvarFormulario = output<TutorFormulario>();

  readonly form = this.fb.nonNullable.group({
    id: [''],
    nome: ['', Validators.required],
    telefone: ['', Validators.required],
    email: ['']
  });

  editando(): boolean {
    return this.form.controls.id.value !== '';
  }

  preencher(tutor: Tutor) {
    this.form.setValue({
      id: tutor.id,
      nome: tutor.nome,
      telefone: tutor.telefone,
      email: tutor.email ?? ''
    });
  }

  limpar() {
    this.form.reset({ id: '', nome: '', telefone: '', email: '' });
  }

  enviar() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const valor = this.form.getRawValue();
    const email = valor.email.trim();
    if (email !== '' && (!email.includes('@') || !email.includes('.'))) {
      this.form.controls.email.setErrors({ email: true });
      return;
    }

    this.salvarFormulario.emit({
      nome: valor.nome.trim(),
      telefone: valor.telefone.trim(),
      email: email === '' ? null : email
    });
  }
}
