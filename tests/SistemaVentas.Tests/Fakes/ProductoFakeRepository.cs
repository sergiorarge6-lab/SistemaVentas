using SistemaVentas.Application.DTOs;
using SistemaVentas.Application.Interfaces;
using SistemaVentas.Domain.Entities;

public class ProductoFakeRepository : IProductoRepository
{
    public Task<int> AgregarAsync(Producto producto)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ActualizarAsync(int id, Producto producto)
    {
        throw new NotImplementedException();
    }

    public Task<bool> EliminarAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Producto?> ObtenerPorIdAsync(int id)
    {
        Producto producto = new Producto
        {
            Id = id,
            Codigo = "P001",
            Nombre = "Mouse",
            Precio = 100,
            Stock = 20,
            Activo = true
        };

        return Task.FromResult<Producto?>(producto);
    }

    public Task<PagedResult<Producto>> ObtenerPaginadoAsync(ProductoFiltroDto filtro)
    {
        throw new NotImplementedException();
    }

    public Task<List<Producto>> ObtenerTodosAsync()
    {
        throw new NotImplementedException();
    }
}