namespace ControleGastos.Api.DTOs;

/// <summary>Totais financeiros consolidados de uma pessoa.</summary>
public record TotalPessoaResponse(
    int PessoaId,
    string Nome,
    decimal TotalReceitas,
    decimal TotalDespesas,
    decimal Saldo);

/// <summary>Resposta da consulta de totais: lista por pessoa e totais gerais.</summary>
public record TotaisResponse(
    IReadOnlyList<TotalPessoaResponse> Pessoas,
    decimal TotalGeralReceitas,
    decimal TotalGeralDespesas,
    decimal SaldoLiquidoGeral);
