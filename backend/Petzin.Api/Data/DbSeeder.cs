using Petzin.Api.Models;

namespace Petzin.Api.Data;

public static class DbSeeder
{
    public static void Seed(AppDbContext db)
    {
        if (db.Tutores.Any())
        {
            return;
        }

        var carlos = new Tutor
        {
            Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            Nome = "Carlos Almeida",
            Telefone = "11999999999",
            Email = "carlos.almeida@email.com"
        };

        var rex = new Pet
        {
            Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
            TutorId = carlos.Id,
            Nome = "Rex",
            Especie = "Cachorro",
            Raca = "Labrador"
        };

        var bolinha = new Pet
        {
            Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
            TutorId = carlos.Id,
            Nome = "Bolinha",
            Especie = "Cachorro",
            Raca = "Poodle"
        };

        var canilGrande = new Canil
        {
            Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
            Numero = 5,
            Porte = "Grande"
        };

        var canilPequeno = new Canil
        {
            Id = Guid.Parse("55555555-5555-5555-5555-555555555555"),
            Numero = 2,
            Porte = "Pequeno"
        };

        db.Tutores.Add(carlos);
        db.Pets.AddRange(rex, bolinha);
        db.Canis.AddRange(canilGrande, canilPequeno);
        db.Historicos.Add(new HistoricoMedico
        {
            Id = Guid.Parse("66666666-6666-6666-6666-666666666666"),
            PetId = rex.Id,
            Data = new DateOnly(2023, 11, 10),
            Descricao = "Vacina em dia. Sem restrições."
        });
        db.Reservas.AddRange(
            new Reserva
            {
                Id = Guid.Parse("77777777-7777-7777-7777-777777777777"),
                PetId = rex.Id,
                CanilId = canilGrande.Id,
                DataEntrada = new DateOnly(2023, 11, 10),
                DataSaida = new DateOnly(2023, 11, 15),
                ValorDiaria = 70m,
                ValorTotal = 350m,
                Status = "Ativa"
            },
            new Reserva
            {
                Id = Guid.Parse("88888888-8888-8888-8888-888888888888"),
                PetId = bolinha.Id,
                CanilId = canilPequeno.Id,
                DataEntrada = new DateOnly(2023, 11, 10),
                DataSaida = new DateOnly(2023, 11, 15),
                ValorDiaria = 50m,
                ValorTotal = 250m,
                Status = "Ativa"
            });
        db.SaveChanges();
    }
}
