namespace ControleGastos.Api.Models;

/// <summary>
/// Representa um membro da residência que pode possuir transações financeiras.
/// O identificador é gerado automaticamente pelo banco (SQLite via EF Core).
/// </summary>
public class Pessoa
{
    /// <summary>Identificador único gerado automaticamente.</summary>
    public int Id { get; set; }

    /// <summary>Nome completo ou apelido da pessoa.</summary>
    public string Nome { get; set; } = string.Empty;

    /// <summary>Idade em anos; usada para restringir receitas de menores de 18.</summary>
    public int Idade { get; set; }

    /// <summary>Transações vinculadas; removidas em cascata ao excluir a pessoa.</summary>
    public ICollection<Transacao> Transacoes { get; set; } = new List<Transacao>();
}
