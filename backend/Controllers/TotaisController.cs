using ControleGastos.Api.DTOs;
using ControleGastos.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace ControleGastos.Api.Controllers;

/// <summary>Endpoint de consulta de totais por pessoa e consolidado geral.</summary>
[ApiController]
[Route("api/[controller]")]
public class TotaisController : ControllerBase
{
    private readonly TotaisService _service;

    public TotaisController(TotaisService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<TotaisResponse>> ObterTotais()
    {
        var totais = await _service.ObterTotaisAsync();
        return Ok(totais);
    }
}
