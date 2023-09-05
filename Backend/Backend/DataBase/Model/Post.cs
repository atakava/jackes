using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.DataBase.Model;

public class Post
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdPosts { get; set; }

    public DateTime CreatedAt { get; set; }

    public int IdUser { get; set; }
    public User User { get; set; }

    public int? IdDeveloper { get; set; }
    public string Title { get; set; }

    public string Text { get; set; }
    public bool InDeveloping { get; set; } = false;

    public ICollection<Comment> Comments { get; set; }
}