namespace ControleGastos.Api.Models;

/// <summary>
/// Registro de uma movimentação financeira (despesa ou receita) associada a uma pessoa.
/// </summary>
public class Transacao
{
    /// <summary>Identificador único gerado automaticamente.</summary>
    public int Id { get; set; }

    /// <summary>Descrição livre da transação (ex.: "Conta de luz").</summary>
    public string Descricao { get; set; } = string.Empty;

    /// <summary>Valor monetário sempre positivo; o tipo indica se é entrada ou saída.</summary>
    public decimal Valor { get; set; }

    /// <summary>Despesa ou receita.</summary>
    public TipoTransacao Tipo { get; set; }

    /// <summary>Chave estrangeira para a pessoa titular da transação.</summary>
    public int PessoaId { get; set; }

    /// <summary>Navegação para a pessoa vinculada.</summary>
    public Pessoa Pessoa { get; set; } = null!;
}
