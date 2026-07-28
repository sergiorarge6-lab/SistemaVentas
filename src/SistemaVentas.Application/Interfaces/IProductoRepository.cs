using SistemaVentas.Domain.Entities;
using SistemaVentas.Application.DTOs;

namespace SistemaVentas.Application.Interfaces;

    public interface IProductoRepository
    {
        Task<Producto?> ObtenerPorIdAsync(int id);
        Task<int> AgregarAsync(Producto producto);
        Task<bool> ActualizarAsync(int id, Producto producto);
        Task<bool> EliminarAsync(int id);
        
        Task<PagedResult<Producto>> ObtenerPaginadoAsync(ProductoFiltroDto filtro);

        Task<List<Producto>> ObtenerTodosAsync();
}
