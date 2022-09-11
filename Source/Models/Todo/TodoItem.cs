using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpeedokuRoyaleServer.Models;

public class TodoItem
{
    public TodoItem() {}

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long? Id { get; set; }

    [Required]
    public string? Task { get; set; }

    [Required]
    public bool IsComplete { get; set; }
}
