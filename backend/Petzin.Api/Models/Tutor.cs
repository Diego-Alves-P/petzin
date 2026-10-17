namespace Petzin.Api.Models;

public class Tutor
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = "";
    public string Telefone { get; set; } = "";
    public string? Email { get; set; }
    public List<Pet> Pets { get; set; } = [];
}
