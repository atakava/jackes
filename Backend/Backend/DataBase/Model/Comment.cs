using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.DataBase.Model;

public class Comment
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CommentId { get; set; }
    public string Text { get; set; }
    public string UserId { get; set; }
    public int PostId { get; set; }
    public User User { get; set; }
   
}
