using Microsoft.EntityFrameworkCore;
using Petzin.Api.Data;
using Petzin.Api.Dtos;
using Petzin.Api.Erros;

namespace Petzin.Api.Services;

public class ReservaService
{
    private static readonly string[] StatusValidos = ["Ativa", "Cancelada"];
    private readonly AppDbContext _db;

    public ReservaService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<ReservaResponse>> Listar()
    {
        return await Consulta()
            .OrderByDescending(r => r.DataEntrada)
            .Select(r => new ReservaResponse(
                r.Id,
                r.PetId,
                r.Pet.Nome,
                r.Pet.Tutor.Nome,
                r.CanilId,
                r.Canil.Numero,
                r.DataEntrada,
                r.DataSaida,
                r.ValorDiaria,
                r.ValorTotal,
                r.Status))
            .ToListAsync();
    }

    public async Task<ReservaResponse> Buscar(Guid id)
    {
        var reserva = await Consulta().FirstOrDefaultAsync(r => r.Id == id);
        if (reserva is null)
        {
            throw new AppException(404, "Reserva não encontrada.");
        }

        return Map(reserva);
    }

    public async Task<ReservaResponse> Criar(ReservaRequest request)
    {
        var dados = await Validar(request, null);
        var reserva = new Models.Reserva
        {
            Id = Guid.NewGuid(),
            PetId = request.PetId,
            CanilId = request.CanilId,
            DataEntrada = dados.Entrada,
            DataSaida = dados.Saida,
            ValorDiaria = dados.Diaria,
            ValorTotal = dados.Total,
            Status = dados.Status
        };
        _db.Reservas.Add(reserva);
        await _db.SaveChangesAsync();
        return await Buscar(reserva.Id);
    }

    public async Task<ReservaResponse> Atualizar(Guid id, ReservaRequest request)
    {
        var reserva = await _db.Reservas.FindAsync(id);
        if (reserva is null)
        {
            throw new AppException(404, "Reserva não encontrada.");
        }

        var dados = await Validar(request, id);
        reserva.PetId = request.PetId;
        reserva.CanilId = request.CanilId;
        reserva.DataEntrada = dados.Entrada;
        reserva.DataSaida = dados.Saida;
        reserva.ValorDiaria = dados.Diaria;
        reserva.ValorTotal = dados.Total;
        reserva.Status = dados.Status;
        await _db.SaveChangesAsync();
        return await Buscar(reserva.Id);
    }

    public async Task Excluir(Guid id)
    {
        var reserva = await _db.Reservas.FindAsync(id);
        if (reserva is null)
        {
            throw new AppException(404, "Reserva não encontrada.");
        }

        _db.Reservas.Remove(reserva);
        await _db.SaveChangesAsync();
    }

    private IQueryable<Models.Reserva> Consulta()
    {
        return _db.Reservas.Include(r => r.Pet).ThenInclude(p => p.Tutor).Include(r => r.Canil);
    }

    private async Task<(DateOnly Entrada, DateOnly Saida, decimal Diaria, decimal Total, string Status)> Validar(
        ReservaRequest request,
        Guid? ignorarId)
    {
        if (request.PetId == Guid.Empty)
        {
            throw new AppException(400, "Informe o pet.");
        }

        if (request.CanilId == Guid.Empty)
        {
            throw new AppException(400, "Informe o canil.");
        }

        if (request.DataEntrada is null)
        {
            throw new AppException(400, "Informe a data de entrada.");
        }

        if (request.DataSaida is null)
        {
            throw new AppException(400, "Informe a data de saída.");
        }

        if (request.ValorDiaria is null)
        {
            throw new AppException(400, "Informe a diária.");
        }

        if (request.DataSaida <= request.DataEntrada)
        {
            throw new AppException(400, "A data de saída deve ser posterior à data de entrada.");
        }

        if (request.ValorDiaria < 0)
        {
            throw new AppException(400, "A diária não pode ser negativa.");
        }

        var status = string.IsNullOrWhiteSpace(request.Status) ? "Ativa" : request.Status.Trim();
        var statusValido = StatusValidos.FirstOrDefault(s => s.Equals(status, StringComparison.OrdinalIgnoreCase));
        if (statusValido is null)
        {
            throw new AppException(400, "O status deve ser Ativa ou Cancelada.");
        }

        var petExiste = await _db.Pets.AnyAsync(p => p.Id == request.PetId);
        if (!petExiste)
        {
            throw new AppException(404, "Pet não encontrado.");
        }

        var canilExiste = await _db.Canis.AnyAsync(c => c.Id == request.CanilId);
        if (!canilExiste)
        {
            throw new AppException(404, "Canil não encontrado.");
        }

        if (statusValido == "Ativa")
        {
            var ocupado = await _db.Reservas.AnyAsync(r =>
                (ignorarId == null || r.Id != ignorarId) &&
                r.CanilId == request.CanilId &&
                r.Status == "Ativa" &&
                r.DataEntrada < request.DataSaida &&
                r.DataSaida > request.DataEntrada);
            if (ocupado)
            {
                throw new AppException(409, "O canil já está ocupado nesse período.");
            }
        }

        var dias = request.DataSaida.Value.DayNumber - request.DataEntrada.Value.DayNumber;
        var total = request.ValorDiaria.Value * dias;
        return (request.DataEntrada.Value, request.DataSaida.Value, request.ValorDiaria.Value, total, statusValido);
    }

    private static ReservaResponse Map(Models.Reserva reserva)
    {
        return new ReservaResponse(
            reserva.Id,
            reserva.PetId,
            reserva.Pet.Nome,
            reserva.Pet.Tutor.Nome,
            reserva.CanilId,
            reserva.Canil.Numero,
            reserva.DataEntrada,
            reserva.DataSaida,
            reserva.ValorDiaria,
            reserva.ValorTotal,
            reserva.Status);
    }
}
