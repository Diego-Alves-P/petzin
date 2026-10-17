import { Component, input, output } from '@angular/core';
import { Canil } from '../../services/canil.service';

@Component({
  selector: 'app-canil-lista',
  templateUrl: './canil-lista.html'
})
export class CanilLista {
  readonly canis = input<Canil[]>([]);
  readonly carregando = input(false);
  readonly editar = output<Canil>();
  readonly excluir = output<Canil>();
}
