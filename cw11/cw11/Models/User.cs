using System.ComponentModel.DataAnnotations;

namespace cw10.DTOs;

public class User
{
    [Key]
    public int IdUser { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
}