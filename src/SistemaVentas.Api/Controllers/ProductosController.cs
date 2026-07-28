// Prueba Git - Rama feature/busqueda-productos
// Cambio realizado en la rama A
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaVentas.Application.DTOs;
using SistemaVentas.Application.Interfaces;
using SistemaVentas.Domain.Entities;



[ApiController]
[Route("api/[controller]")]
public class ProductosController : ControllerBase
{
    private readonly IProductoService _service;
    private readonly ILogger<ProductosController> _logger;



    public ProductosController(IProductoService service, ILogger<ProductosController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet]   
    public async Task<IActionResult> Get(
        [FromQuery] ProductoFiltroDto filtro)
    {
        _logger.LogInformation(
            "Consultando productos. Página={Pagina}, Cantidad={Cantidad}",
            filtro.Pagina,
            filtro.CantidadPorPagina);

        var resultado = await _service.ObtenerPaginadoAsync(filtro);

        return Ok(resultado);
    }

    [HttpGet("todos")]
    public async Task<IActionResult> ObtenerTodos()
    {
        var productos = await _service.ObtenerTodosAsync();

        return Ok(productos);
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<Producto>> ObtenerPorId(int id)
    {
            _logger.LogInformation(
        "Consultando producto Id={Id}",
        id);

        var producto = await _service.ObtenerPorIdAsync(id);

        if (producto == null)
            return NotFound();

        return Ok(producto);
    }

    [HttpPost]
    public async Task<ActionResult> Agregar(CrearProductoDto dto)
    {
            _logger.LogInformation(
        "Creando producto {Codigo}",
        dto.Codigo);

        var id = await _service.AgregarAsync(dto);

        return CreatedAtAction(
            nameof(ObtenerPorId),
            new { id },
            null);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Actualizar(
    int id,
    ActualizarProductoDto dto)
    {
        _logger.LogInformation(
    "Actualizando producto Id={Id}",
    id);

        bool actualizado =
            await _service.ActualizarAsync(id, dto);

        if (!actualizado)
            return NotFound();

        return NoContent();
    }

    [Authorize(Roles = "Administrador")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Eliminar(int id)
    {
            _logger.LogInformation(
        "Eliminando producto Id={Id}",
        id);

        bool eliminado = await _service.EliminarAsync(id);

        if (!eliminado)
            return NotFound();

        return NoContent();
    }



}
