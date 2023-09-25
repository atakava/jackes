using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.DataBase.Model;

public class Post
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int UserId { get; set; }

    public DateTime CreatedAt { get; set; }
    public int? DeveloperId { get; set; }
    public string Title { get; set; }
    public string Text { get; set; }
    public List<string>? Imgs { get; set; }
    public StatusPost StatusPost { get; set; }
    public bool DevelopingId { get; set; }
    public ICollection<Comment> Comments { get; set; }
}