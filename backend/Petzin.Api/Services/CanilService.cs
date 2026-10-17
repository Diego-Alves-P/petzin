using Microsoft.EntityFrameworkCore;
using Petzin.Api.Data;
using Petzin.Api.Dtos;
using Petzin.Api.Erros;
using Petzin.Api.Models;

namespace Petzin.Api.Services;

public class CanilService
{
    private static readonly string[] Portes = ["Pequeno", "Médio", "Grande"];
    private readonly AppDbContext _db;

    public CanilService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<CanilResponse>> Listar()
    {
        return await _db.Canis
            .OrderBy(c => c.Numero)
            .Select(c => new CanilResponse(c.Id, c.Numero, c.Porte))
            .ToListAsync();
    }

    public async Task<CanilResponse> Buscar(Guid id)
    {
        var canil = await _db.Canis.FindAsync(id);
        if (canil is null)
        {
            throw new AppException(404, "Canil não encontrado.");
        }

        return Map(canil);
    }

    public async Task<CanilResponse> Criar(CanilRequest request)
    {
        var porte = NormalizarPorte(request.Porte);
        await GarantirNumeroLivre(request.Numero, null);
        var canil = new Canil
        {
            Id = Guid.NewGuid(),
            Numero = request.Numero,
            Porte = porte
        };
        _db.Canis.Add(canil);
        await _db.SaveChangesAsync();
        return Map(canil);
    }

    public async Task<CanilResponse> Atualizar(Guid id, CanilRequest request)
    {
        var canil = await _db.Canis.FindAsync(id);
        if (canil is null)
        {
            throw new AppException(404, "Canil não encontrado.");
        }

        await GarantirNumeroLivre(request.Numero, id);
        canil.Numero = request.Numero;
        canil.Porte = NormalizarPorte(request.Porte);
        await _db.SaveChangesAsync();
        return Map(canil);
    }

    public async Task Excluir(Guid id)
    {
        var canil = await _db.Canis.FindAsync(id);
        if (canil is null)
        {
            throw new AppException(404, "Canil não encontrado.");
        }

        var possuiReservas = await _db.Reservas.AnyAsync(r => r.CanilId == id);
        if (possuiReservas)
        {
            throw new AppException(409, "Este canil possui reservas e não pode ser excluído.");
        }

        _db.Canis.Remove(canil);
        await _db.SaveChangesAsync();
    }

    private async Task GarantirNumeroLivre(int numero, Guid? ignorarId)
    {
        var existe = await _db.Canis.AnyAsync(c =>
            c.Numero == numero && (ignorarId == null || c.Id != ignorarId));
        if (existe)
        {
            throw new AppException(409, "Já existe um canil com esse número.");
        }
    }

    private static string NormalizarPorte(string porte)
    {
        var valor = porte.Trim();
        var encontrado = Portes.FirstOrDefault(p => p.Equals(valor, StringComparison.OrdinalIgnoreCase));
        if (encontrado is null)
        {
            throw new AppException(400, "O porte deve ser Pequeno, Médio ou Grande.");
        }

        return encontrado;
    }

    private static CanilResponse Map(Canil canil)
    {
        return new CanilResponse(canil.Id, canil.Numero, canil.Porte);
    }
}
