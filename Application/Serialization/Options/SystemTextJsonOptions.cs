using System.Text.Json;
using Application.Interfaces.Serialization.Options;

namespace Application.Serialization.Options
{
    public class SystemTextJsonOptions : IJsonSerializerOptions
    {
        public JsonSerializerOptions JsonSerializerOptions { get; } = new();
    }
}
