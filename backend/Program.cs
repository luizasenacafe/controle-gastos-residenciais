using ControleGastos.Api.Data;
using ControleGastos.Api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// SQLite persiste dados em arquivo local mesmo após encerrar a aplicação.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? "Data Source=controlegastos.db";

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddScoped<PessoaService>();
builder.Services.AddScoped<TransacaoService>();
builder.Services.AddScoped<TotaisService>();

builder.Services.AddControllers();

// Permite que o front-end React (porta 5173) consuma a API durante o desenvolvimento.
builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Cria o banco e tabelas na primeira execução, se ainda não existirem.
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

app.UseCors("Frontend");

app.MapControllers();

app.Run();
