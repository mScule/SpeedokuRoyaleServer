using System.ComponentModel.DataAnnotations;

namespace SpeedokuRoyaleServer.Models;

public class RegisterationData
{
    [Required]
    public string Email { get; set; } = "";

    [Required]
    public string UserName { get; set; } = "";

    [Required]
    public string Password { get; set; } = "";
}
