using System.ComponentModel.DataAnnotations;

namespace Shop_Admin.Models;

public class AdminUser
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; } = "";

    [Required]
    [MaxLength(100)]
    public string LastName { get; set; } = "";

    [Required]
    [MaxLength(100)]
    public string Username { get; set; } = "";

    [Required]
    [MaxLength(150)]
    public string Email { get; set; } = "";

    [Required]
    [MaxLength(255)]
    public string Password { get; set; } = "";

    public string Gender { get; set; } = "Male";

    public DateTime CreatedAt { get; set; }

    public string Status { get; set; } = "Active";
}