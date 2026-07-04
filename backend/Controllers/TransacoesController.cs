using ControleGastos.Api.DTOs;
using ControleGastos.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace ControleGastos.Api.Controllers;

/// <summary>Endpoints REST para registro e listagem de transações financeiras.</summary>
[ApiController]
[Route("api/[controller]")]
public class TransacoesController : ControllerBase
{
    private readonly TransacaoService _service;

    public TransacoesController(TransacaoService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<TransacaoResponse>>> Listar()
    {
        var transacoes = await _service.ListarAsync();
        return Ok(transacoes);
    }

    [HttpPost]
    public async Task<ActionResult<TransacaoResponse>> Criar([FromBody] CriarTransacaoRequest request)
    {
        try
        {
            var transacao = await _service.CriarAsync(request);
            return CreatedAtAction(nameof(Listar), new { id = transacao.Id }, transacao);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }
}
