import { Component, inject, input, output } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MensagemErro } from '../../shared/mensagem-erro';
import { Pet, PetFormulario } from '../../services/pet.service';
import { Tutor } from '../../services/tutor.service';

@Component({
  selector: 'app-pet-form',
  imports: [ReactiveFormsModule, MensagemErro],
  templateUrl: './pet-form.html'
})
export class PetForm {
  private readonly fb = inject(FormBuilder);

  readonly tutores = input<Tutor[]>([]);
  readonly erro = input('');
  readonly salvarFormulario = output<PetFormulario>();

  readonly especies = ['Cachorro', 'Gato', 'Outro'];
  readonly form = this.fb.nonNullable.group({
    id: [''],
    tutorId: ['', Validators.required],
    nome: ['', Validators.required],
    especie: ['', Validators.required],
    raca: ['', Validators.required]
  });

  editando(): boolean {
    return this.form.controls.id.value !== '';
  }

  preencher(pet: Pet) {
    this.form.setValue({
      id: pet.id,
      tutorId: pet.tutorId,
      nome: pet.nome,
      especie: pet.especie,
      raca: pet.raca
    });
  }

  limpar() {
    this.form.reset({ id: '', tutorId: '', nome: '', especie: '', raca: '' });
  }

  enviar() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const valor = this.form.getRawValue();
    this.salvarFormulario.emit({
      tutorId: valor.tutorId,
      nome: valor.nome.trim(),
      especie: valor.especie,
      raca: valor.raca.trim()
    });
  }
}
