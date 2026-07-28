using SistemaVentas.Application.DTOs;
using SistemaVentas.Domain.Entities;

namespace SistemaVentas.Application.Interfaces;

    public interface IProductoService
    {
        Task<Producto?> ObtenerPorIdAsync(int id);
        Task<int> AgregarAsync(CrearProductoDto dto);
        Task<bool> ActualizarAsync(int id, ActualizarProductoDto dto);
        Task<bool> EliminarAsync(int id);
        Task<PagedResult<Producto>> ObtenerPaginadoAsync(ProductoFiltroDto filtro);
        Task<List<Producto>> ObtenerTodosAsync();
}
