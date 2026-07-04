namespace ControleGastos.Api.DTOs;

/// <summary>Dados enviados pelo cliente para criar uma pessoa.</summary>
public record CriarPessoaRequest(string Nome, int Idade);

/// <summary>Representação de pessoa retornada pela API.</summary>
public record PessoaResponse(int Id, string Nome, int Idade);
