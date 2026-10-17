using Microsoft.AspNetCore.Mvc;
using Petzin.Api.Dtos;
using Petzin.Api.Services;

namespace Petzin.Api.Controllers;

[ApiController]
[Route("api/reservas")]
public class ReservasController : ControllerBase
{
    private readonly ReservaService _service;

    public ReservasController(ReservaService service)
    {
        _service = service;
    }

    [HttpGet]
    public Task<List<ReservaResponse>> Listar()
    {
        return _service.Listar();
    }

    [HttpGet("{id:guid}")]
    public Task<ReservaResponse> Buscar(Guid id)
    {
        return _service.Buscar(id);
    }

    [HttpPost]
    public async Task<ActionResult<ReservaResponse>> Criar(ReservaRequest request)
    {
        var criado = await _service.Criar(request);
        return CreatedAtAction(nameof(Buscar), new { id = criado.Id }, criado);
    }

    [HttpPut("{id:guid}")]
    public Task<ReservaResponse> Atualizar(Guid id, ReservaRequest request)
    {
        return _service.Atualizar(id, request);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Excluir(Guid id)
    {
        await _service.Excluir(id);
        return NoContent();
    }
}
