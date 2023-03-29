using System.Text.Json.Serialization;

namespace WebApplication1.Models
{
    public class Cell
    {
        [JsonPropertyName("Value")] public int Value { get; set; }

        [JsonPropertyName("KeyPlayer")] public string KeyPlayer { get; set; }

        [JsonPropertyName("KeyGame")] public string KeyGame { get; set; }

        [JsonPropertyName("View")] public ViewType View { get; set; }
    }

    public enum ViewType
    {
        Circle = 1,
        Сross = 2
    }
}