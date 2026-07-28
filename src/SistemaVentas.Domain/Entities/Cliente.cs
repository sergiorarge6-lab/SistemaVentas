namespace SistemaVentas.Domain.Entities;

public class Cliente
{
    public int Id { get; set; }

    public string Nombre { get; set; } = "";

    public string? Email { get; set; }

    public bool Activo { get; set; }
}