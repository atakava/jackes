using System.ComponentModel.DataAnnotations.Schema;
using Backend.DataBase.Model;

namespace Backend.DataBase.Request;

public class CommentRequest
{
    public string Text { get; set; }
    public List<IFormFile>? Imgs { get; set; }
}