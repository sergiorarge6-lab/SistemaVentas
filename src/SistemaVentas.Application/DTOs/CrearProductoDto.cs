using System.ComponentModel.DataAnnotations;

namespace SistemaVentas.Application.DTOs;

public class CrearProductoDto
{

    public string Codigo { get; set; } = "";


    public string Nombre { get; set; } = "";

    public string? Descripcion { get; set; }

    public decimal Precio { get; set; }

    public int Stock { get; set; }
}
