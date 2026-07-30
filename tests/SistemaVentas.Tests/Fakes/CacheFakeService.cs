using SistemaVentas.Application.Interfaces;

namespace SistemaVentas.Tests.Fakes;

public class CacheFakeService : ICacheService
{
    private readonly Dictionary<string, object> _cache = new();

    public bool TryGetValue<T>(string key, out T? value)
    {
        if (_cache.TryGetValue(key, out var objeto))
        {
            value = (T)objeto;
            return true;
        }

        value = default;
        return false;   
    }

    public void Set<T>(
        string key,
        T value,
        TimeSpan expiration)
    {
        _cache[key] = value!;
    }

    public void Remove(string key)
    {
        _cache.Remove(key);
    }
}