using ControleGastos.Api.Data;
using ControleGastos.Api.DTOs;
using ControleGastos.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ControleGastos.Api.Services;

/// <summary>
/// Agrega receitas, despesas e saldo por pessoa e calcula totais gerais da residência.
/// Saldo = receitas - despesas (positivo indica superávit individual).
/// </summary>
public class TotaisService
{
    private readonly AppDbContext _context;

    public TotaisService(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retorna totais de cada pessoa cadastrada (mesmo sem transações, com zeros)
    /// e, ao final, os totais consolidados de todas as pessoas.
    /// </summary>
    public async Task<TotaisResponse> ObterTotaisAsync()
    {
        var pessoas = await _context.Pessoas
            .Include(p => p.Transacoes)
            .OrderBy(p => p.Nome)
            .ToListAsync();

        var totaisPorPessoa = pessoas.Select(CalcularTotalPessoa).ToList();

        var totalGeralReceitas = totaisPorPessoa.Sum(t => t.TotalReceitas);
        var totalGeralDespesas = totaisPorPessoa.Sum(t => t.TotalDespesas);
        var saldoLiquidoGeral = totalGeralReceitas - totalGeralDespesas;

        return new TotaisResponse(
            totaisPorPessoa,
            totalGeralReceitas,
            totalGeralDespesas,
            saldoLiquidoGeral);
    }

    /// <summary>Soma receitas e despesas de uma pessoa e calcula o saldo individual.</summary>
    private static TotalPessoaResponse CalcularTotalPessoa(Pessoa pessoa)
    {
        var receitas = pessoa.Transacoes
            .Where(t => t.Tipo == TipoTransacao.Receita)
            .Sum(t => t.Valor);

        var despesas = pessoa.Transacoes
            .Where(t => t.Tipo == TipoTransacao.Despesa)
            .Sum(t => t.Valor);

        return new TotalPessoaResponse(
            pessoa.Id,
            pessoa.Nome,
            receitas,
            despesas,
            receitas - despesas);
    }
}
