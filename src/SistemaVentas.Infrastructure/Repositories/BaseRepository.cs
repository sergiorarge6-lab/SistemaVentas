using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace SistemaVentas.Infrastructure.Repositories;

public abstract class BaseRepository
{
    private readonly string _connectionString;

    protected BaseRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("SistemaVentas")!;
    }

    protected SqlConnection CrearConexion()
    {
        return new SqlConnection(_connectionString);
    }
}