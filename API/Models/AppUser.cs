using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace API.Models;

public class AppUser
{
    public int id { get; set; }

    [Required, MaxLength(30)]
    public string name { get; set; } = string.Empty;

    [Required, MaxLength(200)]
    public string PasswordHash { get; set; } = string.Empty;

}