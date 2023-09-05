using Backend.DataBase.Model;

namespace Backend.DataBase.Request;

public class PostRequest
{
    public int? IdDeveloper { get; set; }
    public string Title { get; set; }
    public string Text { get; set; }
    public bool InDeveloping { get; set; } = false;
}