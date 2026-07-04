using ControleGastos.Api.Models;

namespace ControleGastos.Api.DTOs;

/// <summary>Dados enviados pelo cliente para registrar uma transação.</summary>
public record CriarTransacaoRequest(
    string Descricao,
    decimal Valor,
    TipoTransacao Tipo,
    int PessoaId);

/// <summary>Transação retornada na listagem, incluindo nome da pessoa para exibição.</summary>
public record TransacaoResponse(
    int Id,
    string Descricao,
    decimal Valor,
    TipoTransacao Tipo,
    int PessoaId,
    string PessoaNome);
