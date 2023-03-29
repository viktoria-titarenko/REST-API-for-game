using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models;

public class ProcessGame
{
    [Key] public Guid Id { get; set; } = new Guid();
    public string KeyGame { get; set; }

    public string KeyPlayer { get; set; }

    public int Position { get; set; }

    public DateTime createdAt { get; set; }
}