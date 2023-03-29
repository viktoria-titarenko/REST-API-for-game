using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models;

[Index(nameof(KeyPlayer), IsUnique = true)]
public class Keys
{
    [Key] public string KeyPlayer { get; set; }


    public string KeyGame { get; set; }


    public ViewType View { get; set; }
}