namespace SistemaVentas.Application.DTOs.Auth;

public class LoginRequestDto
{
    public string Usuario { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}
