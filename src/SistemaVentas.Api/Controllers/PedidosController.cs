using Microsoft.AspNetCore.Mvc;
using SistemaVentas.Application.DTOs;
using SistemaVentas.Application.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class PedidosController : ControllerBase
{
    private readonly IPedidoService _service;

    public PedidosController(IPedidoService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Crear(CrearPedidoDto dto)
    {
        int id = await _service.CrearPedidoAsync(dto);

        return Ok(id);
    }
}