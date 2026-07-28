

namespace SistemaVentas.Api.BackgroundServices;

public class PedidoBackgroundService : BackgroundService
{
    private readonly ILogger<PedidoBackgroundService> _logger;

    public PedidoBackgroundService(
        ILogger<PedidoBackgroundService> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        _logger.LogInformation("PedidoBackgroundService iniciado.");

        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation(
                "Revisando pedidos pendientes... Hora: {Hora}",
                DateTime.Now);

            await Task.Delay(
                TimeSpan.FromSeconds(60),
                stoppingToken);
        }

        _logger.LogInformation(
            "PedidoBackgroundService finalizado.");
    }
}
