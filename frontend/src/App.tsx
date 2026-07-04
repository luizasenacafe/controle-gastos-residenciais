import { useState } from 'react';
import { PessoasPage } from './pages/PessoasPage';
import { TotaisPage } from './pages/TotaisPage';
import { TransacoesPage } from './pages/TransacoesPage';
import './App.css';

type Aba = 'pessoas' | 'transacoes' | 'totais';

/**
 * Componente raiz: navegação por abas entre as três funcionalidades do sistema.
 */
function App() {
  const [aba, setAba] = useState<Aba>('pessoas');

  return (
    <div className="app">
      <header className="app-header">
        <h1>Controle de Gastos Residenciais</h1>
        <p>Sistema para cadastro de pessoas, transações e consulta de totais.</p>
      </header>

      <nav className="nav-abas">
        <button
          type="button"
          className={aba === 'pessoas' ? 'ativa' : ''}
          onClick={() => setAba('pessoas')}
        >
          Pessoas
        </button>
        <button
          type="button"
          className={aba === 'transacoes' ? 'ativa' : ''}
          onClick={() => setAba('transacoes')}
        >
          Transações
        </button>
        <button
          type="button"
          className={aba === 'totais' ? 'ativa' : ''}
          onClick={() => setAba('totais')}
        >
          Totais
        </button>
      </nav>

      <main>
        {aba === 'pessoas' && <PessoasPage />}
        {aba === 'transacoes' && <TransacoesPage />}
        {aba === 'totais' && <TotaisPage />}
      </main>
    </div>
  );
}

export default App;
