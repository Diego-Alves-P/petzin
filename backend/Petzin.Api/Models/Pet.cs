namespace Petzin.Api.Models;

public class Pet
{
    public Guid Id { get; set; }
    public Guid TutorId { get; set; }
    public Tutor Tutor { get; set; } = null!;
    public string Nome { get; set; } = "";
    public string Especie { get; set; } = "";
    public string Raca { get; set; } = "";
    public List<HistoricoMedico> Historicos { get; set; } = [];
    public List<Reserva> Reservas { get; set; } = [];
}
