namespace SistemaVentas.Domain.Entities;

public class Usuario
{
    public int Id { get; set; }

    public string UsuarioLogin { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string Nombre { get; set; } = string.Empty;

    public string Apellido { get; set; } = string.Empty;

    public string Rol { get; set; } = string.Empty;

    public bool Activo { get; set; }
}