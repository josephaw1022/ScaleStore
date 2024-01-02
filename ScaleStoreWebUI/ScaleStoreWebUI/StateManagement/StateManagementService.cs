using Microsoft.AspNetCore.Components.Server.Circuits;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace ScaleStoreWebUI.StateManagement;

public class StateManagementService(IDistributedCache distributedCache)
{
    public async Task<T?> Get<T>(string key, string circuitId)
    {
        var realKey = $"{circuitId}-{key}";

        var value = await distributedCache.GetStringAsync(realKey) ?? null;

        if (value is null)
        {
            return default;
        }

        return JsonSerializer.Deserialize<T>(value);
    }

    public async Task Set<T>(string key, string circuitId, T value)
    {
        var realKey = $"{circuitId}-{key}";
        await distributedCache.SetStringAsync(realKey, JsonSerializer.Serialize(value));
    }

}
