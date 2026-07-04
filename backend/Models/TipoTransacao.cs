namespace ControleGastos.Api.Models;

/// <summary>
/// Classifica uma transação financeira como despesa (saída) ou receita (entrada).
/// Usado na validação de menores de idade, que só podem registrar despesas.
/// </summary>
public enum TipoTransacao
{
    Despesa = 0,
    Receita = 1
}
