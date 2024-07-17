using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cardmngr.Shared.Extensions;

public static class CommonExtensions
{
    // initialize inner objects individually
    // for example in default constructor some list property initialized with some values,
    // but in 'source' these items are cleaned -
    // without ObjectCreationHandling.Replace default constructor values will be added to result
    private readonly static JsonSerializerOptions _deserializeSettings =
        new() { PreferredObjectCreationHandling = JsonObjectCreationHandling.Replace };

    /// <summary>
    /// Perform a deep Copy of the object, using Json as a serialization method. NOTE: Private members are not cloned using this method.
    /// </summary>
    /// <typeparam name="T">The type of object being copied.</typeparam>
    /// <param name="source">The object instance to copy.</param>
    /// <returns>The copied object.</returns>
    public static T CloneJson<T>(this T source)
    {            
        // Don't serialize a null object, simply return the default for that object
        if (source is null) return default!;

        return JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(source), _deserializeSettings)!;
    }
}
