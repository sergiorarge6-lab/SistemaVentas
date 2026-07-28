using SistemaVentas.Application.DTOs;
using SistemaVentas.Application.Interfaces;
using SistemaVentas.Domain.Entities;


namespace SistemaVentas.Application.Services;

public class ProductoService : IProductoService
{
    private readonly IProductoRepository _productoRepository;
    private readonly ICacheService _cache;
    public ProductoService(IProductoRepository repository, ICacheService cache)
    {
        _productoRepository = repository;
        _cache = cache;;
    }

    public async Task<Producto?> ObtenerPorIdAsync(int id)
    {
        return await _productoRepository.ObtenerPorIdAsync(id);
    }

    public async Task<int> AgregarAsync(CrearProductoDto dto)
    {
        var producto = new Producto
        {
            Codigo = dto.Codigo,
            Nombre = dto.Nombre,
            Descripcion = dto.Descripcion,
            Precio = dto.Precio,
            Stock = dto.Stock,
            Activo = true,
            FechaCreacion = DateTime.Now
        };

        int id = await _productoRepository.AgregarAsync(producto);

        //borro la caché por si alguien despues consulta
        _cache.Remove("Productos_Todos");

        return id;
    }

    public async Task<bool> ActualizarAsync(
    int id,
    ActualizarProductoDto dto)
    {
        var producto = new Producto
        {
            Nombre = dto.Nombre,
            Descripcion = dto.Descripcion,
            Precio = dto.Precio,
            Stock = dto.Stock,
            Activo = dto.Activo,
            FechaModificacion = DateTime.Now
        };

        bool b = await _productoRepository.ActualizarAsync(id, producto);

        //borro la caché por si alguien consulta
        _cache.Remove("Productos_Todos");

        return b;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        bool b = await _productoRepository.EliminarAsync(id);

        //borro la caché por si alguien consulta
        _cache.Remove("Productos_Todos");

        return b;
    }

    public async Task<PagedResult<Producto>> ObtenerPaginadoAsync(
    ProductoFiltroDto filtro)
    {
        return await _productoRepository.ObtenerPaginadoAsync(filtro);
    }


    //esto lo hago así para mostrar el concepto de Memory Cache
    public async Task<List<Producto>> ObtenerTodosAsync()
    {
        const string cacheKey = "Productos_Todos";

        if (_cache.TryGetValue(cacheKey, out List<Producto>? productos))
        {
            return productos!;
        }

        productos = await _productoRepository.ObtenerTodosAsync();

        _cache.Set(
            cacheKey,
            productos,
            TimeSpan.FromMinutes(1));

        return productos;
    }
}