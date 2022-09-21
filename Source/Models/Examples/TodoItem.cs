using System.ComponentModel.DataAnnotations;

namespace SpeedokuRoyaleServer.Models;

public class TodoItem : Indexed
{
    [Required]
    public string? Task { get; set; }

    [Required]
    public bool IsComplete { get; set; }
}
