using System.ComponentModel.DataAnnotations;

namespace NewsApi.Models.Dtos;

public class LoginRequest
{
    [Required(AllowEmptyStrings = false)]
    public required string Username { get; set; }
    [Required(AllowEmptyStrings = false)]
    public required string Password { get; set; }
}