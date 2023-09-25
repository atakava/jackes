using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.DataBase.Model;

public class Team
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    public int AdminId { get; set; }
    public User Admin { get; set; }
    public int LidId { get; set; }
    public string Name { get; set; }
    public string ShortDesc { get; set; }
    public string LongDesc { get; set; }
    
    public List<User> Users { get; set; }
    public List<Post> Posts { get; set; }
    
    public DateTime AtCreate { get; set; }
}