namespace SistemaVentas.Domain.Entities;

public class PedidoDetalle
{
    public int Id { get; set; }

    public int PedidoCabeceraId { get; set; }
    
    // Navegación hacia la cabecera
    public PedidoCabecera PedidoCabecera { get; set; } = null!;

    public int ProductoId { get; set; }
    // Navegación hacia el producto
    public Producto Producto { get; set; } = null!;

    public int Cantidad { get; set; }

    public decimal PrecioUnitario { get; set; }

    public decimal Subtotal { get; set; }

}
