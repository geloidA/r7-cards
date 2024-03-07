using System.Text.Json;

namespace Onlyoffice.Api.Extensions;

public static class StreamExtensions
{
    public static ValueTask<T?> ToAsync<T>(this Stream stream)
    {
        return JsonSerializer.DeserializeAsync<T>(stream);
    }
}
