import { Component, input, output } from '@angular/core';
import { formatarData, formatarDinheiro } from '../../services/formato';
import { Reserva } from '../../services/reserva.service';

@Component({
  selector: 'app-reserva-lista',
  templateUrl: './reserva-lista.html'
})
export class ReservaLista {
  readonly reservas = input<Reserva[]>([]);
  readonly carregando = input(false);
  readonly editar = output<Reserva>();
  readonly excluir = output<Reserva>();
  readonly formatarData = formatarData;
  readonly formatarDinheiro = formatarDinheiro;
}
