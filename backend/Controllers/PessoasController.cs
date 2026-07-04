using ControleGastos.Api.DTOs;
using ControleGastos.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace ControleGastos.Api.Controllers;

/// <summary>Endpoints REST para gerenciamento de pessoas (criar, listar, excluir).</summary>
[ApiController]
[Route("api/[controller]")]
public class PessoasController : ControllerBase
{
    private readonly PessoaService _service;

    public PessoasController(PessoaService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<PessoaResponse>>> Listar()
    {
        var pessoas = await _service.ListarAsync();
        return Ok(pessoas);
    }

    [HttpPost]
    public async Task<ActionResult<PessoaResponse>> Criar([FromBody] CriarPessoaRequest request)
    {
        try
        {
            var pessoa = await _service.CriarAsync(request);
            return CreatedAtAction(nameof(Listar), new { id = pessoa.Id }, pessoa);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Excluir(int id)
    {
        var removida = await _service.ExcluirAsync(id);
        if (!removida)
            return NotFound(new { mensagem = "Pessoa não encontrada." });

        return NoContent();
    }
}
