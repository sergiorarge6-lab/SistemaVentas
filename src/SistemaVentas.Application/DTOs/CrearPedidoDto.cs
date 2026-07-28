namespace SistemaVentas.Application.DTOs;

public class CrearPedidoDto
{
    public int ClienteId { get; set; }

    public List<CrearPedidoDetalleDto> Detalles { get; set; } = [];
}