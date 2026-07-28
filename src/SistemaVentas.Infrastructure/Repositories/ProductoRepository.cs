using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SistemaVentas.Application.DTOs;
using SistemaVentas.Application.Interfaces;
using SistemaVentas.Domain.Entities;


namespace SistemaVentas.Infrastructure.Repositories;

public class ProductoRepository : BaseRepository, IProductoRepository
{
  

    public ProductoRepository(IConfiguration configuration):base(configuration)
    {
       
    }


    public async Task<Producto?> ObtenerPorIdAsync(int id)
    {
        using SqlConnection conexion = CrearConexion();

        await conexion.OpenAsync();

        const string sql = @"
        SELECT Id,
               Nombre,
               Precio,
               Stock,
               Activo
        FROM Productos
        WHERE Id = @Id";

        using SqlCommand comando = new(sql, conexion);

        comando.Parameters.AddWithValue("@Id", id);

        using SqlDataReader reader = await comando.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            return new Producto
            {
                Id = reader.GetInt32(0),
                Nombre = reader.GetString(1),
                Precio = reader.GetDecimal(2),
                Stock = reader.GetInt32(3),
                Activo = reader.GetBoolean(4)
            };
        }

        return null;
    }

    public async Task<int> AgregarAsync(Producto producto)
    {
        using SqlConnection conexion = CrearConexion();

        await conexion.OpenAsync();

        const string sql = @"
    INSERT INTO Productos
    (
        Codigo,
        Nombre,
        Descripcion,
        Precio,
        Stock,
        Activo,
        FechaCreacion
    )
    VALUES
    (
        @Codigo,
        @Nombre,
        @Descripcion,
        @Precio,
        @Stock,
        @Activo,
        @FechaCreacion
    );

    SELECT CAST(SCOPE_IDENTITY() AS INT);";

        using SqlCommand comando = new(sql, conexion);

        comando.Parameters.AddWithValue("@Codigo", producto.Codigo);
        comando.Parameters.AddWithValue("@Nombre", producto.Nombre);

        // Si Descripcion es null, enviar DBNull.Value
        comando.Parameters.AddWithValue(
            "@Descripcion",
            (object?)producto.Descripcion ?? DBNull.Value);

        comando.Parameters.AddWithValue("@Precio", producto.Precio);
        comando.Parameters.AddWithValue("@Stock", producto.Stock);
        comando.Parameters.AddWithValue("@Activo", producto.Activo);
        comando.Parameters.AddWithValue("@FechaCreacion", producto.FechaCreacion);

        return (int)await comando.ExecuteScalarAsync();
    }

    public async Task<bool> ActualizarAsync(int id, Producto producto)
    {
        using SqlConnection conexion = CrearConexion();

        await conexion.OpenAsync();

        const string sql = @"
        UPDATE Productos
        SET
            Nombre = @Nombre,
            Descripcion = @Descripcion,
            Precio = @Precio,
            Stock = @Stock,
            Activo = @Activo,
            FechaModificacion = @FechaModificacion
        WHERE Id = @Id";

        using SqlCommand comando = new(sql, conexion);

        comando.Parameters.AddWithValue("@Id", id);
        comando.Parameters.AddWithValue("@Nombre", producto.Nombre);
        comando.Parameters.AddWithValue(
            "@Descripcion",
            (object?)producto.Descripcion ?? DBNull.Value);
        comando.Parameters.AddWithValue("@Precio", producto.Precio);
        comando.Parameters.AddWithValue("@Stock", producto.Stock);
        comando.Parameters.AddWithValue("@Activo", producto.Activo);
        comando.Parameters.AddWithValue("@FechaModificacion", producto.FechaModificacion);

        int filas = await comando.ExecuteNonQueryAsync();

        return filas > 0;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        using SqlConnection conexion = CrearConexion();

        await conexion.OpenAsync();

        const string sql = @"
        DELETE FROM Productos
        WHERE Id = @Id";

        using SqlCommand comando = new(sql, conexion);

        comando.Parameters.AddWithValue("@Id", id);

        int filas = await comando.ExecuteNonQueryAsync();

        return filas > 0;
    }
    public async Task<PagedResult<Producto>> ObtenerPaginadoAsync(ProductoFiltroDto filtro)
    {
        List<Producto> productos = new();

        using SqlConnection conexion = CrearConexion();

        await conexion.OpenAsync();

        int offset = (filtro.Pagina - 1) * filtro.CantidadPorPagina;

        string where = " WHERE 1=1 ";

        if (!string.IsNullOrWhiteSpace(filtro.Nombre))
            where += " AND Nombre LIKE @Nombre ";

        if (filtro.Activo.HasValue)
            where += " AND Activo = @Activo ";

        if (filtro.PrecioMin.HasValue)
            where += " AND Precio >= @PrecioMin ";

        if (filtro.PrecioMax.HasValue)
            where += " AND Precio <= @PrecioMax ";


        string orderBy = filtro.Orden.ToLower() switch
        {
            "nombre" => "Nombre ASC",
            "nombre_desc" => "Nombre DESC",

            "precio" => "Precio ASC",
            "precio_desc" => "Precio DESC",

            "stock" => "Stock ASC",
            "stock_desc" => "Stock DESC",

            _ => "Nombre ASC"
        };

        string sql = $@"
        SELECT
            Id,
            Codigo,
            Nombre,
            Descripcion,
            Precio,
            Stock,
            Activo,
            FechaCreacion,
            FechaModificacion
        FROM Productos
        {where}
        ORDER BY {orderBy}
        OFFSET @Offset ROWS
        FETCH NEXT @Cantidad ROWS ONLY";

        using SqlCommand comando = new(sql, conexion);

        AgregarParametrosFiltro(comando, filtro);

        comando.Parameters.AddWithValue("@Offset", offset);
        comando.Parameters.AddWithValue("@Cantidad", filtro.CantidadPorPagina);

        using SqlDataReader reader = await comando.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            productos.Add(new Producto
            {
                Id = reader.GetInt32(0),
                Codigo = reader.GetString(1),
                Nombre = reader.GetString(2),
                Descripcion = reader.IsDBNull(3) ? null : reader.GetString(3),
                Precio = reader.GetDecimal(4),
                Stock = reader.GetInt32(5),
                Activo = reader.GetBoolean(6),
                FechaCreacion = reader.GetDateTime(7),
                FechaModificacion = reader.IsDBNull(8) ? null : reader.GetDateTime(8)
            });
        }

        await reader.CloseAsync();

        using SqlCommand comandoTotal =
            new($"SELECT COUNT(*) FROM Productos {where}", conexion);

        AgregarParametrosFiltro(comandoTotal, filtro);

        // ExecuteScalarAsync retorna el primer registro
        int totalRegistros = (int)await comandoTotal.ExecuteScalarAsync();

        return new PagedResult<Producto>
        {
            Items = productos,
            Pagina = filtro.Pagina,
            CantidadPorPagina = filtro.CantidadPorPagina,
            TotalRegistros = totalRegistros
        };
    }

    private static void AgregarParametrosFiltro(
    SqlCommand comando,
    ProductoFiltroDto filtro)
    {
        if (!string.IsNullOrWhiteSpace(filtro.Nombre))
            comando.Parameters.AddWithValue("@Nombre", "%" + filtro.Nombre + "%");

        if (filtro.Activo.HasValue)
            comando.Parameters.AddWithValue("@Activo", filtro.Activo.Value);

        if (filtro.PrecioMin.HasValue)
            comando.Parameters.AddWithValue("@PrecioMin", filtro.PrecioMin.Value);

        if (filtro.PrecioMax.HasValue)
            comando.Parameters.AddWithValue("@PrecioMax", filtro.PrecioMax.Value);
    }

    public Task<List<Producto>> ObtenerTodosAsync()
    {
        throw new NotImplementedException();
    }
}