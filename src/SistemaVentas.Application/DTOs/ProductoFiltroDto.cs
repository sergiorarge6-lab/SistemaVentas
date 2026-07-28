namespace SistemaVentas.Application.DTOs;

public class ProductoFiltroDto
{
    public int Pagina { get; set; } = 1;

    public int CantidadPorPagina { get; set; } = 10;

    public string? Nombre { get; set; }

    public bool? Activo { get; set; }

    public decimal? PrecioMin { get; set; }

    public decimal? PrecioMax { get; set; }
    public string Orden { get; set; } = "nombre";
}