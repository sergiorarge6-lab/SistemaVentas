using SistemaVentas.Application.DTOs;

namespace SistemaVentas.Application.Interfaces;

public interface IPedidoService
{
    Task<int> CrearPedidoAsync(CrearPedidoDto dto);
}
