using Microsoft.EntityFrameworkCore;
using Petzin.Api.Data;
using Petzin.Api.Dtos;
using Petzin.Api.Erros;
using Petzin.Api.Models;

namespace Petzin.Api.Services;

public class PetService
{
    private readonly AppDbContext _db;

    public PetService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<PetResponse>> Listar()
    {
        return await _db.Pets
            .OrderBy(p => p.Nome)
            .Select(p => new PetResponse(p.Id, p.TutorId, p.Tutor.Nome, p.Nome, p.Especie, p.Raca))
            .ToListAsync();
    }

    public async Task<PetResponse> Buscar(Guid id)
    {
        var pet = await _db.Pets.Include(p => p.Tutor).FirstOrDefaultAsync(p => p.Id == id);
        if (pet is null)
        {
            throw new AppException(404, "Pet não encontrado.");
        }

        return Map(pet);
    }

    public async Task<PetResponse> Criar(PetRequest request)
    {
        await GarantirTutor(request.TutorId);
        var pet = new Pet
        {
            Id = Guid.NewGuid(),
            TutorId = request.TutorId,
            Nome = request.Nome.Trim(),
            Especie = request.Especie.Trim(),
            Raca = request.Raca.Trim()
        };
        _db.Pets.Add(pet);
        await _db.SaveChangesAsync();
        return await Buscar(pet.Id);
    }

    public async Task<PetResponse> Atualizar(Guid id, PetRequest request)
    {
        var pet = await _db.Pets.FindAsync(id);
        if (pet is null)
        {
            throw new AppException(404, "Pet não encontrado.");
        }

        await GarantirTutor(request.TutorId);
        pet.TutorId = request.TutorId;
        pet.Nome = request.Nome.Trim();
        pet.Especie = request.Especie.Trim();
        pet.Raca = request.Raca.Trim();
        await _db.SaveChangesAsync();
        return await Buscar(pet.Id);
    }

    public async Task Excluir(Guid id)
    {
        var pet = await _db.Pets.FindAsync(id);
        if (pet is null)
        {
            throw new AppException(404, "Pet não encontrado.");
        }

        var possuiVinculo = await _db.Reservas.AnyAsync(r => r.PetId == id)
            || await _db.Historicos.AnyAsync(h => h.PetId == id);
        if (possuiVinculo)
        {
            throw new AppException(409, "Este pet possui histórico ou reservas e não pode ser excluído.");
        }

        _db.Pets.Remove(pet);
        await _db.SaveChangesAsync();
    }

    public async Task<List<HistoricoResponse>> ListarHistorico(Guid petId)
    {
        await GarantirPet(petId);
        return await _db.Historicos
            .Where(h => h.PetId == petId)
            .OrderByDescending(h => h.Data)
            .Select(h => new HistoricoResponse(h.Id, h.PetId, h.Data, h.Descricao))
            .ToListAsync();
    }

    public async Task<HistoricoResponse> CriarHistorico(Guid petId, HistoricoRequest request)
    {
        await GarantirPet(petId);
        if (request.Data is null)
        {
            throw new AppException(400, "Informe a data.");
        }

        var historico = new HistoricoMedico
        {
            Id = Guid.NewGuid(),
            PetId = petId,
            Data = request.Data.Value,
            Descricao = request.Descricao.Trim()
        };
        _db.Historicos.Add(historico);
        await _db.SaveChangesAsync();
        return new HistoricoResponse(historico.Id, historico.PetId, historico.Data, historico.Descricao);
    }

    private async Task GarantirTutor(Guid tutorId)
    {
        if (tutorId == Guid.Empty)
        {
            throw new AppException(400, "Informe o tutor.");
        }

        var existe = await _db.Tutores.AnyAsync(t => t.Id == tutorId);
        if (!existe)
        {
            throw new AppException(404, "Tutor não encontrado.");
        }
    }

    private async Task GarantirPet(Guid petId)
    {
        var existe = await _db.Pets.AnyAsync(p => p.Id == petId);
        if (!existe)
        {
            throw new AppException(404, "Pet não encontrado.");
        }
    }

    private static PetResponse Map(Pet pet)
    {
        return new PetResponse(pet.Id, pet.TutorId, pet.Tutor.Nome, pet.Nome, pet.Especie, pet.Raca);
    }
}
