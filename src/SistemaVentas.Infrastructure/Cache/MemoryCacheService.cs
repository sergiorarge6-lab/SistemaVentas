using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using SistemaVentas.Application.Interfaces;

namespace SistemaVentas.Infrastructure.Cache;

public class MemoryCacheService : ICacheService
{
    private readonly IMemoryCache _cache;
    private readonly ILogger<MemoryCacheService> _logger;

    public MemoryCacheService(
        IMemoryCache cache,
        ILogger<MemoryCacheService> logger)
    {
        _cache = cache;
        _logger = logger;
    }

    public bool TryGetValue<T>(
        string key,
        out T? value)
    {
        bool encontrado = _cache.TryGetValue(key, out value);

        if (encontrado)
        {
            _logger.LogInformation(
                "Leyendo '{Key}' desde la caché.",
                key);
        }

        return encontrado;
    }

    public void Set<T>(
        string key,
        T value,
        TimeSpan expiration)
    {
        _logger.LogInformation(
            "Guardando '{Key}' en la caché.",
            key);

        _cache.Set(
            key,
            value,
            expiration);
    }

    public void Remove(string key)
    {
        _logger.LogInformation(
            "Eliminando '{Key}' de la caché.",
            key);

        _cache.Remove(key);
    }
}