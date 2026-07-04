/**
 * Formata valores monetários em Real brasileiro para exibição nas tabelas.
 */
export function formatarMoeda(valor: number): string {
  return valor.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' });
}

/**
 * Retorna classe CSS condicional para saldos positivos (verde) ou negativos (vermelho).
 */
export function classeSaldo(valor: number): string {
  if (valor > 0) return 'positivo';
  if (valor < 0) return 'negativo';
  return '';
}
