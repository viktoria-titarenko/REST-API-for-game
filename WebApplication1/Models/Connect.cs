using System.Text.Json.Serialization;

namespace WebApplication1.Models
{
    public class Connect
    {
        [JsonPropertyName("KeyGame")] public string KeyGame { get; set; }

        [JsonPropertyName("Type")] public ViewType Type { get; set; }
    }
}