# Controle de Gastos Residenciais

Sistema para controle de gastos da residência, com cadastro de pessoas, registro de transações (despesas/receitas) e consulta de totais consolidados.

## Tecnologias

| Camada    | Stack                                      |
|-----------|--------------------------------------------|
| Back-end  | .NET 10, C#, ASP.NET Core Web API, EF Core |
| Front-end | React 19, TypeScript, Vite                 |
| Persistência | SQLite (arquivo `controlegastos.db`)    |

Os dados permanecem salvos no arquivo SQLite mesmo após fechar a aplicação.

## Funcionalidades

### Pessoas
- Criar, listar e excluir pessoas (ID, nome, idade)
- ID gerado automaticamente
- Ao excluir uma pessoa, todas as transações dela são removidas

### Transações
- Criar e listar transações (ID, descrição, valor, tipo, pessoa)
- Pessoa deve existir no cadastro
- Menores de 18 anos só podem cadastrar **despesas**

### Totais
- Lista cada pessoa com total de receitas, despesas e saldo (receita − despesa)
- Exibe totais gerais ao final da listagem

## Como executar

### 1. Back-end (API)

```bash
cd backend
dotnet run
```

A API sobe em **http://localhost:5140**.

### 2. Front-end

Em outro terminal:

```bash
cd frontend
npm install
npm run dev
```

Acesse **http://localhost:5173** no navegador.

> Certifique-se de que a API esteja rodando antes de usar o front-end.

## Estrutura do projeto

```
controle-gastos-residenciais/
├── backend/                 # API .NET
│   ├── Controllers/         # Endpoints REST
│   ├── Data/                # DbContext (SQLite)
│   ├── DTOs/                # Objetos de transferência
│   ├── Models/              # Entidades de domínio
│   └── Services/            # Regras de negócio
└── frontend/                # React + TypeScript
    └── src/
        ├── pages/           # Telas (Pessoas, Transações, Totais)
        ├── services/        # Cliente HTTP da API
        └── types/           # Tipos TypeScript
```

## Endpoints da API

| Método | Rota              | Descrição                    |
|--------|-------------------|------------------------------|
| GET    | /api/pessoas      | Lista pessoas                |
| POST   | /api/pessoas      | Cria pessoa                  |
| DELETE | /api/pessoas/{id} | Exclui pessoa e transações   |
| GET    | /api/transacoes   | Lista transações             |
| POST   | /api/transacoes   | Cria transação               |
| GET    | /api/totais       | Consulta totais consolidados |

## Publicar no GitHub

```bash
git add .
git commit -m "Implementa sistema de controle de gastos residenciais"
git remote add origin https://github.com/SEU_USUARIO/controle-gastos-residenciais.git
git push -u origin main
```

Adicione `controlegastos.db` ao `.gitignore` se preferir não versionar dados locais.
