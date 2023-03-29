using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models;

public class Winners
{
    [Key] public string KeyGame { get; set; }

    public ViewType Winner { get; set; }
}