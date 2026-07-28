using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaVentas.Application.DTOs.Auth;
using SistemaVentas.Application.Interfaces.Security;
using SistemaVentas.Domain.Entities;

namespace SistemaVentas.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IJwtService _jwtService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IJwtService jwtService, ILogger<AuthController> logger)
    {
        _jwtService = jwtService;
        _logger = logger;
    }

    // Simulación de una tabla Usuarios
    private static readonly List<Usuario> _usuarios =
    [
        new Usuario
        {
            Id = 1,
            UsuarioLogin = "admin",
            Password = "1234",
            Nombre = "Sergio",
            Apellido = "Rossi",
            Rol = "Administrador",
            Activo = true
        },
        new Usuario
        {
            Id = 2,
            UsuarioLogin = "juan",
            Password = "1234",
            Nombre = "Juan",
            Apellido = "Pérez",
            Rol = "Vendedor",
            Activo = true
        },
        new Usuario
        {
            Id = 3,
            UsuarioLogin = "maria",
            Password = "1234",
            Nombre = "María",
            Apellido = "Gómez",
            Rol = "Supervisor",
            Activo = true
        },
        new Usuario
        {
            Id = 4,
            UsuarioLogin = "ana",
            Password = "1234",
            Nombre = "Ana",
            Apellido = "López",
            Rol = "Cliente",
            Activo = true
        },
        new Usuario
        {
            Id = 5,
            UsuarioLogin = "pedro",
            Password = "1234",
            Nombre = "Pedro",
            Apellido = "Martínez",
            Rol = "Cliente",
            Activo = false
        }
    ];

    [HttpPost("login")]
    public ActionResult<LoginResponseDto> Login(LoginRequestDto request)
    {
        Usuario? usuario = _usuarios.FirstOrDefault(u =>
            u.UsuarioLogin == request.Usuario &&
            u.Password == request.Password);

        _logger.LogInformation("login del usuario {usu}",request.Usuario);


        if (usuario is null)
        {
            return Unauthorized("Usuario o contraseña incorrectos.");
        }

        if (!usuario.Activo)
        {
            return Unauthorized("El usuario está deshabilitado.");
        }

        string token = _jwtService.GenerarToken(
            usuario.Id,
            usuario.UsuarioLogin,
            usuario.Rol);

        return Ok(new LoginResponseDto
        {
            Token = token
        });
    }


    // devuelvel el perfil (tiene que estar logueado)
    [Authorize]
    [HttpGet("perfil")]
    public IActionResult Perfil()
    {
        return Ok(new
        {
            Mensaje = "Accediste al perfil.",
            Usuario = User.Identity?.Name
        });
    }

    // para prueba JWT - Solo administrador
    [Authorize(Roles = "Administrador")]
    [HttpDelete("eliminar")]
    public IActionResult Eliminar()
    {
        return Ok("Aceptado.Solo un administrador puede ejecutar esta acción.");
    }
}