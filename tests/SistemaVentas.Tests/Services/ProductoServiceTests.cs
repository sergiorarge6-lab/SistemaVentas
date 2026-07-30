using SistemaVentas.Application.Services;
using SistemaVentas.Tests.Fakes;

namespace SistemaVentas.Tests.Services;

public class ProductoServiceTests
{
    [Fact]
    public async Task ObtenerPorIdDebeRetornarProducto()
    {
        // Arrange

        var repository = new ProductoFakeRepository();

        var cache = new CacheFakeService();

        var service =
            new ProductoService(repository, cache);

        // Act

        var producto =
            await service.ObtenerPorIdAsync(1);

        // Assert

        Assert.NotNull(producto);

        Assert.Equal(1, producto!.Id);

        Assert.Equal("Mouse", producto.Nombre);

        Assert.Equal(100, producto.Precio);
    }
}