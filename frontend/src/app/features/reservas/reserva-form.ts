import { Component, inject, input, output } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { formatarDinheiro } from '../../services/formato';
import { Canil } from '../../services/canil.service';
import { Pet } from '../../services/pet.service';
import { Reserva, ReservaFormulario } from '../../services/reserva.service';
import { MensagemErro } from '../../shared/mensagem-erro';

@Component({
  selector: 'app-reserva-form',
  imports: [ReactiveFormsModule, MensagemErro],
  templateUrl: './reserva-form.html'
})
export class ReservaForm {
  private readonly fb = inject(FormBuilder);

  readonly pets = input<Pet[]>([]);
  readonly canis = input<Canil[]>([]);
  readonly erro = input('');
  readonly salvarFormulario = output<ReservaFormulario>();
  readonly statusOpcoes = ['Ativa', 'Cancelada'];

  readonly form = this.fb.nonNullable.group({
    id: [''],
    petId: ['', Validators.required],
    canilId: ['', Validators.required],
    dataEntrada: ['', Validators.required],
    dataSaida: ['', Validators.required],
    valorDiaria: [0, [Validators.required, Validators.min(0)]],
    status: ['Ativa', Validators.required]
  });

  editando(): boolean {
    return this.form.controls.id.value !== '';
  }

  totalPrevisto(): string {
    const entrada = this.form.controls.dataEntrada.value;
    const saida = this.form.controls.dataSaida.value;
    const diaria = Number(this.form.controls.valorDiaria.value);
    if (!entrada || !saida || Number.isNaN(diaria) || diaria < 0) {
      return '';
    }

    const dias = (new Date(`${saida}T00:00:00`).getTime() - new Date(`${entrada}T00:00:00`).getTime()) / 86400000;
    if (dias <= 0) {
      return '';
    }

    return formatarDinheiro(dias * diaria);
  }

  preencher(reserva: Reserva) {
    this.form.setValue({
      id: reserva.id,
      petId: reserva.petId,
      canilId: reserva.canilId,
      dataEntrada: reserva.dataEntrada,
      dataSaida: reserva.dataSaida,
      valorDiaria: reserva.valorDiaria,
      status: reserva.status
    });
  }

  limpar() {
    this.form.reset({
      id: '',
      petId: '',
      canilId: '',
      dataEntrada: '',
      dataSaida: '',
      valorDiaria: 0,
      status: 'Ativa'
    });
  }

  enviar() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const valor = this.form.getRawValue();
    this.salvarFormulario.emit({
      petId: valor.petId,
      canilId: valor.canilId,
      dataEntrada: valor.dataEntrada,
      dataSaida: valor.dataSaida,
      valorDiaria: Number(valor.valorDiaria),
      status: valor.status
    });
  }
}
