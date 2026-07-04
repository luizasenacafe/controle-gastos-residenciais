import { useCallback, useEffect, useState } from 'react';
import { totaisApi } from '../services/api';
import type { TotaisResponse } from '../types';
import { classeSaldo, formatarMoeda } from '../utils/formatacao';

/**
 * Consulta de totais: exibe receitas, despesas e saldo por pessoa,
 * seguido do consolidado geral de toda a residência.
 */
export function TotaisPage() {
  const [totais, setTotais] = useState<TotaisResponse | null>(null);
  const [erro, setErro] = useState<string | null>(null);

  const carregar = useCallback(async () => {
    try {
      setTotais(await totaisApi.obter());
      setErro(null);
    } catch (e) {
      setErro(e instanceof Error ? e.message : 'Erro ao carregar totais.');
    }
  }, []);

  useEffect(() => {
    carregar();
  }, [carregar]);

  return (
    <section className="pagina">
      <header className="pagina-header">
        <h2>Totais</h2>
        <p>Resumo financeiro por pessoa e consolidado geral da residência.</p>
      </header>

      <button type="button" className="btn-secundario" onClick={carregar}>
        Atualizar
      </button>

      {erro && <p className="mensagem-erro">{erro}</p>}

      {totais && (
        <div className="tabela-wrapper">
          <table>
            <thead>
              <tr>
                <th>Pessoa</th>
                <th>Receitas</th>
                <th>Despesas</th>
                <th>Saldo</th>
              </tr>
            </thead>
            <tbody>
              {totais.pessoas.length === 0 ? (
                <tr>
                  <td colSpan={4} className="vazio">
                    Nenhuma pessoa cadastrada.
                  </td>
                </tr>
              ) : (
                totais.pessoas.map((p) => (
                  <tr key={p.pessoaId}>
                    <td>{p.nome}</td>
                    <td className="positivo">{formatarMoeda(p.totalReceitas)}</td>
                    <td className="negativo">{formatarMoeda(p.totalDespesas)}</td>
                    <td className={classeSaldo(p.saldo)}>{formatarMoeda(p.saldo)}</td>
                  </tr>
                ))
              )}
            </tbody>
            {totais.pessoas.length > 0 && (
              <tfoot>
                <tr className="total-geral">
                  <td><strong>Total geral</strong></td>
                  <td className="positivo">
                    <strong>{formatarMoeda(totais.totalGeralReceitas)}</strong>
                  </td>
                  <td className="negativo">
                    <strong>{formatarMoeda(totais.totalGeralDespesas)}</strong>
                  </td>
                  <td className={classeSaldo(totais.saldoLiquidoGeral)}>
                    <strong>{formatarMoeda(totais.saldoLiquidoGeral)}</strong>
                  </td>
                </tr>
              </tfoot>
            )}
          </table>
        </div>
      )}
    </section>
  );
}
