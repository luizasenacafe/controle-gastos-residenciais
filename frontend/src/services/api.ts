/**
 * Cliente HTTP centralizado para comunicação com a API .NET.
 * Todas as chamadas passam por aqui, facilitando manutenção da URL base.
 */
import type {
  ApiError,
  CriarPessoaRequest,
  CriarTransacaoRequest,
  Pessoa,
  TotaisResponse,
  Transacao,
} from '../types';

const API_BASE = import.meta.env.VITE_API_URL ?? 'http://localhost:5140/api';

async function request<T>(path: string, options?: RequestInit): Promise<T> {
  const response = await fetch(`${API_BASE}${path}`, {
    headers: { 'Content-Type': 'application/json', ...options?.headers },
    ...options,
  });

  if (!response.ok) {
    const error = (await response.json().catch(() => null)) as ApiError | null;
    throw new Error(error?.mensagem ?? `Erro na requisição (${response.status})`);
  }

  if (response.status === 204) {
    return undefined as T;
  }

  return response.json() as Promise<T>;
}

/** Operações de cadastro de pessoas */
export const pessoasApi = {
  listar: () => request<Pessoa[]>('/pessoas'),
  criar: (data: CriarPessoaRequest) =>
    request<Pessoa>('/pessoas', { method: 'POST', body: JSON.stringify(data) }),
  excluir: (id: number) => request<void>(`/pessoas/${id}`, { method: 'DELETE' }),
};

/** Operações de cadastro de transações */
export const transacoesApi = {
  listar: () => request<Transacao[]>('/transacoes'),
  criar: (data: CriarTransacaoRequest) =>
    request<Transacao>('/transacoes', { method: 'POST', body: JSON.stringify(data) }),
};

/** Consulta de totais por pessoa e consolidado geral */
export const totaisApi = {
  obter: () => request<TotaisResponse>('/totais'),
};
