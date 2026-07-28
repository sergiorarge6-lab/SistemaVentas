using SistemaVentas.Application.DTOs;

namespace SistemaVentas.Application.Interfaces;

public interface IPedidoRepository
{
    Task<int> CrearPedidoAsync(CrearPedidoDto dto);
}