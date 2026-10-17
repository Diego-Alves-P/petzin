using System.ComponentModel.DataAnnotations;

namespace Petzin.Api.Dtos;

public class TutorRequest
{
    [Required(ErrorMessage = "Informe o nome.")]
    [MaxLength(120, ErrorMessage = "O nome deve ter no máximo 120 caracteres.")]
    public string Nome { get; set; } = "";

    [Required(ErrorMessage = "Informe o telefone.")]
    [MaxLength(20, ErrorMessage = "O telefone deve ter no máximo 20 caracteres.")]
    public string Telefone { get; set; } = "";

    [MaxLength(160, ErrorMessage = "O e-mail deve ter no máximo 160 caracteres.")]
    public string? Email { get; set; }
}

public record TutorResponse(Guid Id, string Nome, string Telefone, string? Email);

public class PetRequest
{
    [Required(ErrorMessage = "Informe o tutor.")]
    public Guid TutorId { get; set; }

    [Required(ErrorMessage = "Informe o nome do pet.")]
    [MaxLength(120, ErrorMessage = "O nome deve ter no máximo 120 caracteres.")]
    public string Nome { get; set; } = "";

    [Required(ErrorMessage = "Informe a espécie.")]
    [MaxLength(40, ErrorMessage = "A espécie deve ter no máximo 40 caracteres.")]
    public string Especie { get; set; } = "";

    [Required(ErrorMessage = "Informe a raça.")]
    [MaxLength(80, ErrorMessage = "A raça deve ter no máximo 80 caracteres.")]
    public string Raca { get; set; } = "";
}

public record PetResponse(Guid Id, Guid TutorId, string TutorNome, string Nome, string Especie, string Raca);

public class HistoricoRequest
{
    [Required(ErrorMessage = "Informe a data.")]
    public DateOnly? Data { get; set; }

    [Required(ErrorMessage = "Informe a descrição.")]
    [MaxLength(500, ErrorMessage = "A descrição deve ter no máximo 500 caracteres.")]
    public string Descricao { get; set; } = "";
}

public record HistoricoResponse(Guid Id, Guid PetId, DateOnly Data, string Descricao);

public class CanilRequest
{
    [Range(1, 999, ErrorMessage = "Informe um número de canil válido.")]
    public int Numero { get; set; }

    [Required(ErrorMessage = "Informe o porte.")]
    [MaxLength(20, ErrorMessage = "O porte deve ter no máximo 20 caracteres.")]
    public string Porte { get; set; } = "";
}

public record CanilResponse(Guid Id, int Numero, string Porte);

public class ReservaRequest
{
    [Required(ErrorMessage = "Informe o pet.")]
    public Guid PetId { get; set; }

    [Required(ErrorMessage = "Informe o canil.")]
    public Guid CanilId { get; set; }

    [Required(ErrorMessage = "Informe a data de entrada.")]
    public DateOnly? DataEntrada { get; set; }

    [Required(ErrorMessage = "Informe a data de saída.")]
    public DateOnly? DataSaida { get; set; }

    [Required(ErrorMessage = "Informe a diária.")]
    public decimal? ValorDiaria { get; set; }

    [MaxLength(20, ErrorMessage = "O status deve ter no máximo 20 caracteres.")]
    public string Status { get; set; } = "Ativa";
}

public record ReservaResponse(
    Guid Id,
    Guid PetId,
    string PetNome,
    string TutorNome,
    Guid CanilId,
    int CanilNumero,
    DateOnly DataEntrada,
    DateOnly DataSaida,
    decimal ValorDiaria,
    decimal ValorTotal,
    string Status);
