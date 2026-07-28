using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SistemaVentas.Application.DTOs;
using SistemaVentas.Application.Interfaces;
using SistemaVentas.Domain.Entities;
using SistemaVentas.Infrastructure.Data;

namespace SistemaVentas.Infrastructure.Repositories;

public class PedidoEfRepository : IPedidoRepository
{
    private readonly SistemaVentasDbContext _context;
    private readonly ILogger<PedidoEfRepository> _logger;

    public PedidoEfRepository(SistemaVentasDbContext context, ILogger<PedidoEfRepository> logger)
    {
        _context = context;
        _logger = logger;
    }


    public async Task<int> CrearPedidoAsync(CrearPedidoDto dto)
    {

        _logger.LogInformation("creando pedido para el cliente: {clie}",dto.ClienteId);

        PedidoCabecera pedido = new()
        {
            ClienteId = dto.ClienteId,
            Fecha = DateTime.Now,
            Total = 0
        };

        // Obtengo todos los productos de una sola vez
        List<int> idsProductos = dto.Detalles
            .Select(d => d.ProductoId)
            .Distinct()
            .ToList();

        //para que sea mas rápida la búsqueda (ToDictionaryAsync)
        Dictionary<int, Producto> productos = await _context.Productos
            .Where(p => idsProductos.Contains(p.Id))
            .ToDictionaryAsync(p => p.Id);

        foreach (var item in dto.Detalles)
        {
            if (!productos.TryGetValue(item.ProductoId, out Producto? producto))
            {
                throw new Exception(
                    $"No existe el producto {item.ProductoId}");
            }

            //verifico el stock
            if (producto.Stock < item.Cantidad)
            {
                throw new Exception(
                    $"Stock insuficiente para el producto {producto.Id}");
            }

            PedidoDetalle detalle = new()
            {
                ProductoId = producto.Id,
                Cantidad = item.Cantidad,
                PrecioUnitario = producto.Precio,
                Subtotal = producto.Precio * item.Cantidad
            };

            pedido.Detalles.Add(detalle);

            producto.Stock -= item.Cantidad;

            pedido.Total += detalle.Subtotal;
        }

        _context.PedidoCabeceras.Add(pedido);

        await _context.SaveChangesAsync();

        return pedido.Id;
    }

}