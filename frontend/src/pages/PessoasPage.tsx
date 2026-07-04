import { useCallback, useEffect, useState } from 'react';
import { pessoasApi } from '../services/api';
import type { Pessoa } from '../types';

/**
 * Tela de gerenciamento de pessoas: criação, listagem e exclusão.
 * Ao excluir, o back-end remove automaticamente todas as transações vinculadas.
 */
export function PessoasPage() {
  const [pessoas, setPessoas] = useState<Pessoa[]>([]);
  const [nome, setNome] = useState('');
  const [idade, setIdade] = useState('');
  const [erro, setErro] = useState<string | null>(null);
  const [carregando, setCarregando] = useState(false);

  const carregar = useCallback(async () => {
    try {
      setPessoas(await pessoasApi.listar());
    } catch (e) {
      setErro(e instanceof Error ? e.message : 'Erro ao carregar pessoas.');
    }
  }, []);

  useEffect(() => {
    carregar();
  }, [carregar]);

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault();
    setErro(null);
    setCarregando(true);

    try {
      await pessoasApi.criar({ nome, idade: Number(idade) });
      setNome('');
      setIdade('');
      await carregar();
    } catch (e) {
      setErro(e instanceof Error ? e.message : 'Erro ao criar pessoa.');
    } finally {
      setCarregando(false);
    }
  }

  async function handleExcluir(id: number, nomePessoa: string) {
    if (!confirm(`Excluir "${nomePessoa}" e todas as transações dela?`)) return;

    setErro(null);
    try {
      await pessoasApi.excluir(id);
      await carregar();
    } catch (e) {
      setErro(e instanceof Error ? e.message : 'Erro ao excluir pessoa.');
    }
  }

  return (
    <section className="pagina">
      <header className="pagina-header">
        <h2>Pessoas</h2>
        <p>Cadastre os moradores da residência. A exclusão remove também suas transações.</p>
      </header>

      <form className="formulario" onSubmit={handleSubmit}>
        <div className="campo">
          <label htmlFor="nome">Nome</label>
          <input
            id="nome"
            value={nome}
            onChange={(e) => setNome(e.target.value)}
            placeholder="Ex.: Maria Silva"
            required
          />
        </div>
        <div className="campo">
          <label htmlFor="idade">Idade</label>
          <input
            id="idade"
            type="number"
            min={0}
            value={idade}
            onChange={(e) => setIdade(e.target.value)}
            placeholder="Ex.: 25"
            required
          />
        </div>
        <button type="submit" disabled={carregando}>
          {carregando ? 'Salvando...' : 'Cadastrar pessoa'}
        </button>
      </form>

      {erro && <p className="mensagem-erro">{erro}</p>}

      <div className="tabela-wrapper">
        <table>
          <thead>
            <tr>
              <th>ID</th>
              <th>Nome</th>
              <th>Idade</th>
              <th>Ações</th>
            </tr>
          </thead>
          <tbody>
            {pessoas.length === 0 ? (
              <tr>
                <td colSpan={4} className="vazio">
                  Nenhuma pessoa cadastrada.
                </td>
              </tr>
            ) : (
              pessoas.map((p) => (
                <tr key={p.id}>
                  <td>{p.id}</td>
                  <td>{p.nome}</td>
                  <td>{p.idade}</td>
                  <td>
                    <button
                      type="button"
                      className="btn-perigo"
                      onClick={() => handleExcluir(p.id, p.nome)}
                    >
                      Excluir
                    </button>
                  </td>
                </tr>
              ))
            )}
          </tbody>
        </table>
      </div>
    </section>
  );
}
