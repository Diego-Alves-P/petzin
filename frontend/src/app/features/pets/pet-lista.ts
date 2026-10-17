import { Component, input, output } from '@angular/core';
import { Pet } from '../../services/pet.service';

@Component({
  selector: 'app-pet-lista',
  templateUrl: './pet-lista.html'
})
export class PetLista {
  readonly pets = input<Pet[]>([]);
  readonly carregando = input(false);
  readonly editar = output<Pet>();
  readonly historico = output<Pet>();
  readonly excluir = output<Pet>();
}
