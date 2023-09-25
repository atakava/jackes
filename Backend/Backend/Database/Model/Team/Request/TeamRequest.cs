namespace Backend.DataBase.Model.Request;

public class TeamRequest
{
    public int AdminId { get; set; }
    public string Name { get; set; }
    public string ShortDesc { get; set; }
    public string LongDesc { get; set; }
    public List<int> UserIds { get; set; }
}