using Moq;
using SistemaVentas.Application.Interfaces;
using SistemaVentas.Application.Services;
using SistemaVentas.Domain.Entities;
using SistemaVentas.Tests.Fakes;

namespace SistemaVentas.Tests.Services;

public class ProductoServiceMoqTests
{
    [Fact]
    public async Task ObtenerPorIdDebeRetornarProducto()
    {
        // Arrange

        var productoEsperado = new Producto
        {
            Id = 1,
            Codigo = "P001",
            Nombre = "Mouse",
            Precio = 100,
            Stock = 20,
            Activo = true
        };

        Mock<IProductoRepository> repository = new Mock<IProductoRepository>();
        var cache = new CacheFakeService();

        repository
        .Setup(r => r.ObtenerPorIdAsync(1))
        .ReturnsAsync(productoEsperado);

   
        var service =
            new ProductoService(repository.Object, cache);


        // Act
        var producto =
            await service.ObtenerPorIdAsync(1);

        // Assert

        Assert.NotNull(producto);

        Assert.Equal("Mouse", producto!.Nombre);

        Assert.Equal(productoEsperado, producto);
    }
}