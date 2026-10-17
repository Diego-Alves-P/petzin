namespace Petzin.Api.Models;

public class Canil
{
    public Guid Id { get; set; }
    public int Numero { get; set; }
    public string Porte { get; set; } = "";
    public List<Reserva> Reservas { get; set; } = [];
}
