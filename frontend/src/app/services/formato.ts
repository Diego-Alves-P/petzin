import { HttpErrorResponse } from '@angular/common/http';

export function lerErro(erro: HttpErrorResponse): string {
  const corpo = erro.error;
  if (corpo && typeof corpo === 'object' && 'errors' in corpo && corpo.errors) {
    const grupos = Object.values(corpo.errors as Record<string, unknown>);
    for (const grupo of grupos) {
      if (Array.isArray(grupo) && grupo.length > 0) {
        return String(grupo[0]);
      }
    }
  }

  if (corpo && typeof corpo === 'object') {
    if ('detail' in corpo && typeof corpo.detail === 'string' && corpo.detail) {
      return corpo.detail;
    }
    if ('title' in corpo && typeof corpo.title === 'string' && corpo.title) {
      return corpo.title;
    }
  }

  return 'Não foi possível concluir a operação.';
}

export function formatarData(valor: string): string {
  const [ano, mes, dia] = valor.split('-');
  if (!ano || !mes || !dia) {
    return valor;
  }

  return `${dia}/${mes}/${ano}`;
}

export function formatarDinheiro(valor: number): string {
  return valor.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' });
}
