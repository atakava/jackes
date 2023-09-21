using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.DataBase.Model;

public class PostImg
{
   [Key]
   [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
   public int Id { get; set; }
   public int PostId { get; set; }
   public string ImageUrl { get; set; }
}