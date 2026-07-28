using SistemaVentas.Application.DTOs;
using SistemaVentas.Application.Interfaces;

namespace SistemaVentas.Application.Services;

public class PedidoService : IPedidoService
{
    private readonly IPedidoRepository _pedidoRepository;

    public PedidoService(IPedidoRepository pedidoRepository)
    {
        _pedidoRepository = pedidoRepository;
    }

    public async Task<int> CrearPedidoAsync(CrearPedidoDto dto)
    {
        return await _pedidoRepository.CrearPedidoAsync(dto);
    }
}