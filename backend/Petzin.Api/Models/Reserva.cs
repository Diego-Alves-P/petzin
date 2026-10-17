namespace Petzin.Api.Models;

public class Reserva
{
    public Guid Id { get; set; }
    public Guid PetId { get; set; }
    public Pet Pet { get; set; } = null!;
    public Guid CanilId { get; set; }
    public Canil Canil { get; set; } = null!;
    public DateOnly DataEntrada { get; set; }
    public DateOnly DataSaida { get; set; }
    public decimal ValorDiaria { get; set; }
    public decimal ValorTotal { get; set; }
    public string Status { get; set; } = "Ativa";
}
