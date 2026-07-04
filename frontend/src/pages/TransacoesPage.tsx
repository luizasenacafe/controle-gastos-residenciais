import { useCallback, useEffect, useMemo, useState } from 'react';
import { pessoasApi, transacoesApi } from '../services/api';
import { TipoTransacao, type Pessoa, type Transacao } from '../types';
import { formatarMoeda } from '../utils/formatacao';

/**
 * Tela de transações: criação e listagem.
 * O formulário desabilita "Receita" quando a pessoa selecionada é menor de 18 anos,
 * refletindo a regra de negócio implementada também no back-end.
 */
export function TransacoesPage() {
  const [pessoas, setPessoas] = useState<Pessoa[]>([]);
  const [transacoes, setTransacoes] = useState<Transacao[]>([]);
  const [descricao, setDescricao] = useState('');
  const [valor, setValor] = useState('');
  const [tipo, setTipo] = useState<TipoTransacao>(TipoTransacao.Despesa);
  const [pessoaId, setPessoaId] = useState('');
  const [erro, setErro] = useState<string | null>(null);
  const [carregando, setCarregando] = useState(false);

  const pessoaSelecionada = useMemo(
    () => pessoas.find((p) => p.id === Number(pessoaId)),
    [pessoas, pessoaId],
  );

  /** Menores de 18 só podem registrar despesas */
  const somenteDespesa = pessoaSelecionada !== undefined && pessoaSelecionada.idade < 18;

  const carregar = useCallback(async () => {
    try {
      const [listaPessoas, listaTransacoes] = await Promise.all([
        pessoasApi.listar(),
        transacoesApi.listar(),
      ]);
      setPessoas(listaPessoas);
      setTransacoes(listaTransacoes);
    } catch (e) {
      setErro(e instanceof Error ? e.message : 'Erro ao carregar dados.');
    }
  }, []);

  useEffect(() => {
    carregar();
  }, [carregar]);

  // Garante que o tipo permaneça como despesa se a pessoa for menor de idade
  useEffect(() => {
    if (somenteDespesa && tipo === TipoTransacao.Receita) {
      setTipo(TipoTransacao.Despesa);
    }
  }, [somenteDespesa, tipo]);

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault();
    setErro(null);
    setCarregando(true);

    try {
      await transacoesApi.criar({
        descricao,
        valor: Number(valor),
        tipo,
        pessoaId: Number(pessoaId),
      });
      setDescricao('');
      setValor('');
      setTipo(TipoTransacao.Despesa);
      await carregar();
    } catch (e) {
      setErro(e instanceof Error ? e.message : 'Erro ao criar transação.');
    } finally {
      setCarregando(false);
    }
  }

  function labelTipo(t: TipoTransacao): string {
    return t === TipoTransacao.Receita ? 'Receita' : 'Despesa';
  }

  return (
    <section className="pagina">
      <header className="pagina-header">
        <h2>Transações</h2>
        <p>Registre despesas e receitas vinculadas a uma pessoa cadastrada.</p>
      </header>

      <form className="formulario formulario-largo" onSubmit={handleSubmit}>
        <div className="campo">
          <label htmlFor="descricao">Descrição</label>
          <input
            id="descricao"
            value={descricao}
            onChange={(e) => setDescricao(e.target.value)}
            placeholder="Ex.: Supermercado"
            required
          />
        </div>
        <div className="campo">
          <label htmlFor="valor">Valor (R$)</label>
          <input
            id="valor"
            type="number"
            min={0.01}
            step={0.01}
            value={valor}
            onChange={(e) => setValor(e.target.value)}
            placeholder="0,00"
            required
          />
        </div>
        <div className="campo">
          <label htmlFor="pessoa">Pessoa</label>
          <select
            id="pessoa"
            value={pessoaId}
            onChange={(e) => setPessoaId(e.target.value)}
            required
          >
            <option value="">Selecione...</option>
            {pessoas.map((p) => (
              <option key={p.id} value={p.id}>
                {p.nome} ({p.idade} anos)
              </option>
            ))}
          </select>
        </div>
        <div className="campo">
          <label htmlFor="tipo">Tipo</label>
          <select
            id="tipo"
            value={tipo}
            onChange={(e) => setTipo(Number(e.target.value) as TipoTransacao)}
          >
            <option value={TipoTransacao.Despesa}>Despesa</option>
            <option value={TipoTransacao.Receita} disabled={somenteDespesa}>
              Receita{somenteDespesa ? ' (indisponível para menores)' : ''}
            </option>
          </select>
        </div>
        <button type="submit" disabled={carregando || pessoas.length === 0}>
          {carregando ? 'Salvando...' : 'Cadastrar transação'}
        </button>
      </form>

      {pessoas.length === 0 && (
        <p className="aviso">Cadastre ao menos uma pessoa antes de registrar transações.</p>
      )}

      {somenteDespesa && (
        <p className="aviso">
          {pessoaSelecionada?.nome} é menor de 18 anos — apenas despesas são permitidas.
        </p>
      )}

      {erro && <p className="mensagem-erro">{erro}</p>}

      <div className="tabela-wrapper">
        <table>
          <thead>
            <tr>
              <th>ID</th>
              <th>Descrição</th>
              <th>Valor</th>
              <th>Tipo</th>
              <th>Pessoa</th>
            </tr>
          </thead>
          <tbody>
            {transacoes.length === 0 ? (
              <tr>
                <td colSpan={5} className="vazio">
                  Nenhuma transação registrada.
                </td>
              </tr>
            ) : (
              transacoes.map((t) => (
                <tr key={t.id}>
                  <td>{t.id}</td>
                  <td>{t.descricao}</td>
                  <td>{formatarMoeda(t.valor)}</td>
                  <td>
                    <span className={t.tipo === TipoTransacao.Receita ? 'badge receita' : 'badge despesa'}>
                      {labelTipo(t.tipo)}
                    </span>
                  </td>
                  <td>{t.pessoaNome}</td>
                </tr>
              ))
            )}
          </tbody>
        </table>
      </div>
    </section>
  );
}
