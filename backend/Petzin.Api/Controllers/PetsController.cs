using Microsoft.AspNetCore.Mvc;
using Petzin.Api.Dtos;
using Petzin.Api.Services;

namespace Petzin.Api.Controllers;

[ApiController]
[Route("api/pets")]
public class PetsController : ControllerBase
{
    private readonly PetService _service;

    public PetsController(PetService service)
    {
        _service = service;
    }

    [HttpGet]
    public Task<List<PetResponse>> Listar()
    {
        return _service.Listar();
    }

    [HttpGet("{id:guid}")]
    public Task<PetResponse> Buscar(Guid id)
    {
        return _service.Buscar(id);
    }

    [HttpPost]
    public async Task<ActionResult<PetResponse>> Criar(PetRequest request)
    {
        var criado = await _service.Criar(request);
        return CreatedAtAction(nameof(Buscar), new { id = criado.Id }, criado);
    }

    [HttpPut("{id:guid}")]
    public Task<PetResponse> Atualizar(Guid id, PetRequest request)
    {
        return _service.Atualizar(id, request);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Excluir(Guid id)
    {
        await _service.Excluir(id);
        return NoContent();
    }

    [HttpGet("{id:guid}/historico")]
    public Task<List<HistoricoResponse>> ListarHistorico(Guid id)
    {
        return _service.ListarHistorico(id);
    }

    [HttpPost("{id:guid}/historico")]
    public async Task<ActionResult<HistoricoResponse>> CriarHistorico(Guid id, HistoricoRequest request)
    {
        var criado = await _service.CriarHistorico(id, request);
        return Created($"/api/pets/{id}/historico/{criado.Id}", criado);
    }
}
