using Microsoft.AspNetCore.Mvc;
using Petzin.Api.Dtos;
using Petzin.Api.Services;

namespace Petzin.Api.Controllers;

[ApiController]
[Route("api/canis")]
public class CanisController : ControllerBase
{
    private readonly CanilService _service;

    public CanisController(CanilService service)
    {
        _service = service;
    }

    [HttpGet]
    public Task<List<CanilResponse>> Listar()
    {
        return _service.Listar();
    }

    [HttpGet("{id:guid}")]
    public Task<CanilResponse> Buscar(Guid id)
    {
        return _service.Buscar(id);
    }

    [HttpPost]
    public async Task<ActionResult<CanilResponse>> Criar(CanilRequest request)
    {
        var criado = await _service.Criar(request);
        return CreatedAtAction(nameof(Buscar), new { id = criado.Id }, criado);
    }

    [HttpPut("{id:guid}")]
    public Task<CanilResponse> Atualizar(Guid id, CanilRequest request)
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
