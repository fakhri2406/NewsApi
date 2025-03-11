using System.ComponentModel.DataAnnotations;

namespace NewsApi.Models.Dtos;

public class RegisterRequest
{
    [Required(AllowEmptyStrings = false)]
    public required string Username { get; set; }
    [Required(AllowEmptyStrings = false)]
    public required string Password { get; set; }
    [Required]
    public int RoleId { get; set; }
}