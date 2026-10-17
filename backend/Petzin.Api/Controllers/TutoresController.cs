using Microsoft.AspNetCore.Mvc;
using Petzin.Api.Dtos;
using Petzin.Api.Services;

namespace Petzin.Api.Controllers;

[ApiController]
[Route("api/tutores")]
public class TutoresController : ControllerBase
{
    private readonly TutorService _service;

    public TutoresController(TutorService service)
    {
        _service = service;
    }

    [HttpGet]
    public Task<List<TutorResponse>> Listar()
    {
        return _service.Listar();
    }

    [HttpGet("{id:guid}")]
    public Task<TutorResponse> Buscar(Guid id)
    {
        return _service.Buscar(id);
    }

    [HttpPost]
    public async Task<ActionResult<TutorResponse>> Criar(TutorRequest request)
    {
        var criado = await _service.Criar(request);
        return CreatedAtAction(nameof(Buscar), new { id = criado.Id }, criado);
    }

    [HttpPut("{id:guid}")]
    public Task<TutorResponse> Atualizar(Guid id, TutorRequest request)
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
