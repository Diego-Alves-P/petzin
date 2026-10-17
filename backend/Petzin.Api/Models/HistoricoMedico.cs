namespace Petzin.Api.Models;

public class HistoricoMedico
{
    public Guid Id { get; set; }
    public Guid PetId { get; set; }
    public Pet Pet { get; set; } = null!;
    public DateOnly Data { get; set; }
    public string Descricao { get; set; } = "";
}
