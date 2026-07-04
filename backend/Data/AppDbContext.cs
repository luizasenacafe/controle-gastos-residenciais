using ControleGastos.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ControleGastos.Api.Data;

/// <summary>
/// Contexto do Entity Framework Core. Utiliza SQLite para persistir dados
/// em arquivo local (controlegastos.db), mantendo informações após fechar a aplicação.
/// </summary>
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Pessoa> Pessoas => Set<Pessoa>();
    public DbSet<Transacao> Transacoes => Set<Transacao>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Ao excluir uma pessoa, todas as transações dela são removidas automaticamente.
        modelBuilder.Entity<Transacao>()
            .HasOne(t => t.Pessoa)
            .WithMany(p => p.Transacoes)
            .HasForeignKey(t => t.PessoaId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Pessoa>()
            .Property(p => p.Nome)
            .HasMaxLength(200)
            .IsRequired();

        modelBuilder.Entity<Transacao>()
            .Property(t => t.Descricao)
            .HasMaxLength(500)
            .IsRequired();

        modelBuilder.Entity<Transacao>()
            .Property(t => t.Valor)
            .HasPrecision(18, 2);
    }
}
