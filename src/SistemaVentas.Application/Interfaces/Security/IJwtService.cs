namespace SistemaVentas.Application.Interfaces.Security;

public interface IJwtService
{
    string GenerarToken(
        int id,
        string usuario,
        string rol);
}