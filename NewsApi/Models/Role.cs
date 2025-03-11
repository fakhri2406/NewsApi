using System.ComponentModel.DataAnnotations;

namespace NewsApi.Models;

public class Role
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
    public IEnumerable<User>? Users { get; set; } = new List<User>();
}