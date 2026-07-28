using Microsoft.EntityFrameworkCore;
using SistemaVentas.Application.DTOs;
using SistemaVentas.Application.Interfaces;
using SistemaVentas.Domain.Entities;
using SistemaVentas.Infrastructure.Data;

namespace SistemaVentas.Infrastructure.Repositories;


public class ProductoEfRepository: IProductoRepository
{
    private readonly SistemaVentasDbContext _context;

    public ProductoEfRepository(SistemaVentasDbContext context)
    {
        _context = context;
    }
    public async Task<bool> ActualizarAsync(int id, Producto producto)
    {

        //hago esto para que solo haga el update de los campos que fueron modificados
        //Change Tracking (mantiene vigilado los cambios)
        var productoActual = await _context.Productos.FindAsync(id);

        if (productoActual == null)
            return false;

        productoActual.Nombre = producto.Nombre;
        productoActual.Descripcion = producto.Descripcion;
        productoActual.Precio = producto.Precio;
        productoActual.Stock = producto.Stock;
        productoActual.Activo = producto.Activo;
        productoActual.FechaModificacion = DateTime.Now;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<int> AgregarAsync(Producto producto)
    {
        _context.Productos.Add(producto);

        await _context.SaveChangesAsync();

        return producto.Id;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var producto = await _context.Productos.FindAsync(id);

        if (producto == null)
            return false;

        _context.Productos.Remove(producto);

        await _context.SaveChangesAsync();

        return true;
    }

    public async  Task<PagedResult<Producto>> ObtenerPaginadoAsync(ProductoFiltroDto filtro)
    {
        // le agrego AsNoTracking para que no quede "vigilado" por DbContext y sea mas rápido.
        IQueryable<Producto> query = _context.Productos.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filtro.Nombre))
        {
            query = query.Where(p => p.Nombre.Contains(filtro.Nombre));
        }

        if (filtro.Activo.HasValue)
        {
            query = query.Where(p => p.Activo == filtro.Activo.Value);
        }

        query = filtro.Orden.ToLower() switch
        {
            "codigo" => query.OrderBy(p => p.Codigo),
            "precio" => query.OrderBy(p => p.Precio),
            "stock" => query.OrderBy(p => p.Stock),
            _ => query.OrderBy(p => p.Nombre)
        };

        int totalRegistros = await query.CountAsync();

        List<Producto> productos = await query
            .Skip((filtro.Pagina - 1) * filtro.CantidadPorPagina)
            .Take(filtro.CantidadPorPagina)
            .ToListAsync();

        return new PagedResult<Producto>
        {
            Items = productos,  
            TotalRegistros = totalRegistros,
            Pagina = filtro.Pagina,
            CantidadPorPagina = filtro.CantidadPorPagina
        };
    }
    public async Task<Producto?> ObtenerPorIdAsync(int id)
    {
        return await _context.Productos
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<List<Producto>> ObtenerTodosAsync()
    {
        return await _context.Productos
           .Where(p => p.Activo)
           .Where(p => p.Stock > 10)
           .OrderBy(p => p.Nombre)
           .ToListAsync();
    }



}