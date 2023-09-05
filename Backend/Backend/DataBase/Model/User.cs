using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.DataBase.Model;

public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdUser { get; set; }
    public string Login { get; set; }
    public string Mail { get; set; }
    public string Password { get; set; }
    public string Avatar { get; set; }
    public string? Role { get; set; }
    public ICollection<Comment>? Comments { get; set; }
}