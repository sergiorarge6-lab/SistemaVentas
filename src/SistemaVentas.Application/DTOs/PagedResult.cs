namespace SistemaVentas.Application.DTOs;

public class PagedResult<T>
{
    public List<T> Items { get; set; } = new();

    public int TotalRegistros { get; set; }

    public int Pagina { get; set; }

    public int CantidadPorPagina { get; set; }

    public int TotalPaginas =>
        (int)Math.Ceiling((double)TotalRegistros / CantidadPorPagina);
}