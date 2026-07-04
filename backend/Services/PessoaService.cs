using ControleGastos.Api.Data;
using ControleGastos.Api.DTOs;
using ControleGastos.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ControleGastos.Api.Services;

/// <summary>
/// Regras de negócio para cadastro e exclusão de pessoas.
/// A exclusão remove em cascata todas as transações vinculadas (configurado no DbContext).
/// </summary>
public class PessoaService
{
    private readonly AppDbContext _context;

    public PessoaService(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>Lista todas as pessoas ordenadas por nome.</summary>
    public async Task<IReadOnlyList<PessoaResponse>> ListarAsync()
    {
        return await _context.Pessoas
            .OrderBy(p => p.Nome)
            .Select(p => new PessoaResponse(p.Id, p.Nome, p.Idade))
            .ToListAsync();
    }

    /// <summary>Cria uma nova pessoa com identificador gerado pelo banco.</summary>
    public async Task<PessoaResponse> CriarAsync(CriarPessoaRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Nome))
            throw new ArgumentException("O nome é obrigatório.");

        if (request.Idade < 0)
            throw new ArgumentException("A idade não pode ser negativa.");

        var pessoa = new Pessoa
        {
            Nome = request.Nome.Trim(),
            Idade = request.Idade
        };

        _context.Pessoas.Add(pessoa);
        await _context.SaveChangesAsync();

        return new PessoaResponse(pessoa.Id, pessoa.Nome, pessoa.Idade);
    }

    /// <summary>
    /// Remove a pessoa e, por cascata, todas as suas transações.
    /// </summary>
    public async Task<bool> ExcluirAsync(int id)
    {
        var pessoa = await _context.Pessoas.FindAsync(id);
        if (pessoa is null)
            return false;

        _context.Pessoas.Remove(pessoa);
        await _context.SaveChangesAsync();
        return true;
    }

    /// <summary>Verifica se a pessoa existe (usado na validação de transações).</summary>
    public async Task<Pessoa?> ObterPorIdAsync(int id)
    {
        return await _context.Pessoas.FindAsync(id);
    }
}
