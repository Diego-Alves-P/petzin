import { Component, input } from '@angular/core';

@Component({
  selector: 'app-mensagem-erro',
  template: `
    @if (texto()) {
      <p class="rounded border border-red-200 bg-red-50 px-3 py-2 text-sm text-red-700">{{ texto() }}</p>
    }
  `
})
export class MensagemErro {
  readonly texto = input('');
}
