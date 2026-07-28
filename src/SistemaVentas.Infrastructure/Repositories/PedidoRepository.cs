using Microsoft.Extensions.Configuration;
using SistemaVentas.Application.DTOs;
using SistemaVentas.Application.Interfaces;

using Microsoft.Data.SqlClient;
using SistemaVentas.Domain.Entities;

namespace SistemaVentas.Infrastructure.Repositories;

public class PedidoRepository : BaseRepository, IPedidoRepository
{
    public PedidoRepository(IConfiguration configuration)
        : base(configuration)
    {
    }

    public async Task<int> CrearPedidoAsync(CrearPedidoDto dto)
    {
        using SqlConnection conexion = CrearConexion();

        await conexion.OpenAsync();

        using SqlTransaction transaction = conexion.BeginTransaction();

        try
        {
            const string sql = @"
            INSERT INTO PedidoCabecera
            (
                ClienteId,
                Fecha,
                Total
            )
            VALUES
            (
                @ClienteId,
                @Fecha,
                @Total
            );

            SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using SqlCommand comando = new(sql, conexion, transaction);

            comando.Parameters.AddWithValue("@ClienteId", dto.ClienteId);
            comando.Parameters.AddWithValue("@Fecha", DateTime.Now);
            comando.Parameters.AddWithValue("@Total", 0);

            int pedidoId = (int)await comando.ExecuteScalarAsync();

            foreach (var item in dto.Detalles)
            {
                // Obtener precio
                const string sqlPrecio = @"
                SELECT Precio
                FROM Productos
                WHERE Id = @ProductoId";

                using SqlCommand cmdPrecio =
                    new(sqlPrecio, conexion, transaction);

                cmdPrecio.Parameters.AddWithValue("@ProductoId", item.ProductoId);

                decimal precio = (decimal)await cmdPrecio.ExecuteScalarAsync();

                decimal subtotal = precio * item.Cantidad;

                // Verificar stock
                const string sqlStockActual = @"
                SELECT Stock
                FROM Productos
                WHERE Id = @ProductoId";

                using SqlCommand cmdStockActual =
                    new(sqlStockActual, conexion, transaction);

                cmdStockActual.Parameters.AddWithValue("@ProductoId", item.ProductoId);

                int stockActual = (int)await cmdStockActual.ExecuteScalarAsync();

                if (stockActual < item.Cantidad)
                {
                    throw new Exception(
                        $"Stock insuficiente para el producto {item.ProductoId}");
                }

                // Insertar detalle
                const string sqlDetalle = @"
                INSERT INTO PedidoDetalle
                (
                    PedidoCabeceraId,
                    ProductoId,
                    Cantidad,
                    PrecioUnitario,
                    Subtotal
                )
                VALUES
                (
                    @PedidoCabeceraId,
                    @ProductoId,
                    @Cantidad,
                    @PrecioUnitario,
                    @Subtotal
                );";

                using SqlCommand cmdDetalle =
                    new(sqlDetalle, conexion, transaction);

                cmdDetalle.Parameters.AddWithValue("@PedidoCabeceraId", pedidoId);
                cmdDetalle.Parameters.AddWithValue("@ProductoId", item.ProductoId);
                cmdDetalle.Parameters.AddWithValue("@Cantidad", item.Cantidad);
                cmdDetalle.Parameters.AddWithValue("@PrecioUnitario", precio);
                cmdDetalle.Parameters.AddWithValue("@Subtotal", subtotal);

                await cmdDetalle.ExecuteNonQueryAsync();

                // Descontar stock
                const string sqlStock = @"
                UPDATE Productos
                SET Stock = Stock - @Cantidad
                WHERE Id = @ProductoId";

                using SqlCommand cmdStock =
                    new(sqlStock, conexion, transaction);

                cmdStock.Parameters.AddWithValue("@ProductoId", item.ProductoId);
                cmdStock.Parameters.AddWithValue("@Cantidad", item.Cantidad);

                await cmdStock.ExecuteNonQueryAsync();
            }

            transaction.Commit();

            return pedidoId;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }
}