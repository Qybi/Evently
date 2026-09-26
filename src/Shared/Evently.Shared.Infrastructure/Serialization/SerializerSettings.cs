using Newtonsoft.Json;

namespace Evently.Shared.Infrastructure.Serialization;

public static class SerializerSettings
{
    public static readonly JsonSerializerSettings Instance = new()
    {
        TypeNameHandling = TypeNameHandling.All,
        // fixes jsonb serialization for types, so they get serialized in the actual object structure
        MetadataPropertyHandling = MetadataPropertyHandling.ReadAhead
    };
}
