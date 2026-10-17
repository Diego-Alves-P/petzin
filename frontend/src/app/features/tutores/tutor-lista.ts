import { Component, input, output } from '@angular/core';
import { Tutor } from '../../services/tutor.service';

@Component({
  selector: 'app-tutor-lista',
  templateUrl: './tutor-lista.html'
})
export class TutorLista {
  readonly tutores = input<Tutor[]>([]);
  readonly carregando = input(false);
  readonly editar = output<Tutor>();
  readonly excluir = output<Tutor>();
}
