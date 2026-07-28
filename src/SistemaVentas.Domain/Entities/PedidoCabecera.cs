namespace SistemaVentas.Domain.Entities;

public class PedidoCabecera
{
    public int Id { get; set; }

    public int ClienteId { get; set; }

    public DateTime Fecha { get; set; }

    public decimal Total { get; set; }

    public List<PedidoDetalle> Detalles { get; set; } = new();
}