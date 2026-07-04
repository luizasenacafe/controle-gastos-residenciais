using ControleGastos.Api.Data;
using ControleGastos.Api.DTOs;
using ControleGastos.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ControleGastos.Api.Services;

/// <summary>
/// Regras de negócio para registro e listagem de transações.
/// Menores de 18 anos só podem registrar despesas (não receitas).
/// </summary>
public class TransacaoService
{
    private const int IdadeMinimaParaReceita = 18;

    private readonly AppDbContext _context;
    private readonly PessoaService _pessoaService;

    public TransacaoService(AppDbContext context, PessoaService pessoaService)
    {
        _context = context;
        _pessoaService = pessoaService;
    }

    /// <summary>Lista transações com nome da pessoa, da mais recente para a mais antiga.</summary>
    public async Task<IReadOnlyList<TransacaoResponse>> ListarAsync()
    {
        return await _context.Transacoes
            .Include(t => t.Pessoa)
            .OrderByDescending(t => t.Id)
            .Select(t => new TransacaoResponse(
                t.Id,
                t.Descricao,
                t.Valor,
                t.Tipo,
                t.PessoaId,
                t.Pessoa.Nome))
            .ToListAsync();
    }

    /// <summary>
    /// Registra uma transação validando existência da pessoa, valor positivo
    /// e restrição de receita para menores de idade.
    /// </summary>
    public async Task<TransacaoResponse> CriarAsync(CriarTransacaoRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Descricao))
            throw new ArgumentException("A descrição é obrigatória.");

        if (request.Valor <= 0)
            throw new ArgumentException("O valor deve ser maior que zero.");

        var pessoa = await _pessoaService.ObterPorIdAsync(request.PessoaId)
            ?? throw new ArgumentException("Pessoa não encontrada.");

        // Regra de negócio: menores de 18 só podem cadastrar despesas.
        if (pessoa.Idade < IdadeMinimaParaReceita && request.Tipo == TipoTransacao.Receita)
            throw new ArgumentException(
                "Pessoas menores de 18 anos só podem cadastrar despesas.");

        var transacao = new Transacao
        {
            Descricao = request.Descricao.Trim(),
            Valor = request.Valor,
            Tipo = request.Tipo,
            PessoaId = request.PessoaId
        };

        _context.Transacoes.Add(transacao);
        await _context.SaveChangesAsync();

        return new TransacaoResponse(
            transacao.Id,
            transacao.Descricao,
            transacao.Valor,
            transacao.Tipo,
            transacao.PessoaId,
            pessoa.Nome);
    }
}
