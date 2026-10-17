using Microsoft.EntityFrameworkCore;
using Petzin.Api.Data;
using Petzin.Api.Dtos;
using Petzin.Api.Erros;
using Petzin.Api.Models;

namespace Petzin.Api.Services;

public class TutorService
{
    private readonly AppDbContext _db;

    public TutorService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<TutorResponse>> Listar()
    {
        return await _db.Tutores
            .OrderBy(t => t.Nome)
            .Select(t => new TutorResponse(t.Id, t.Nome, t.Telefone, t.Email))
            .ToListAsync();
    }

    public async Task<TutorResponse> Buscar(Guid id)
    {
        var tutor = await _db.Tutores.FindAsync(id);
        if (tutor is null)
        {
            throw new AppException(404, "Tutor não encontrado.");
        }

        return Map(tutor);
    }

    public async Task<TutorResponse> Criar(TutorRequest request)
    {
        var tutor = new Tutor
        {
            Id = Guid.NewGuid(),
            Nome = request.Nome.Trim(),
            Telefone = request.Telefone.Trim(),
            Email = EmailOuNulo(request.Email)
        };
        _db.Tutores.Add(tutor);
        await _db.SaveChangesAsync();
        return Map(tutor);
    }

    public async Task<TutorResponse> Atualizar(Guid id, TutorRequest request)
    {
        var tutor = await _db.Tutores.FindAsync(id);
        if (tutor is null)
        {
            throw new AppException(404, "Tutor não encontrado.");
        }

        tutor.Nome = request.Nome.Trim();
        tutor.Telefone = request.Telefone.Trim();
        tutor.Email = EmailOuNulo(request.Email);
        await _db.SaveChangesAsync();
        return Map(tutor);
    }

    public async Task Excluir(Guid id)
    {
        var tutor = await _db.Tutores.FindAsync(id);
        if (tutor is null)
        {
            throw new AppException(404, "Tutor não encontrado.");
        }

        var possuiPets = await _db.Pets.AnyAsync(p => p.TutorId == id);
        if (possuiPets)
        {
            throw new AppException(409, "Este tutor possui pets e não pode ser excluído.");
        }

        _db.Tutores.Remove(tutor);
        await _db.SaveChangesAsync();
    }

    private static TutorResponse Map(Tutor tutor)
    {
        return new TutorResponse(tutor.Id, tutor.Nome, tutor.Telefone, tutor.Email);
    }

    private static string? EmailOuNulo(string? email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return null;
        }

        var valor = email.Trim();
        if (!valor.Contains('@') || !valor.Contains('.'))
        {
            throw new AppException(400, "Informe um e-mail válido.");
        }

        return valor;
    }
}
