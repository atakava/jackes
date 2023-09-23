namespace Backend.DataBase.Request;

public class CommentUpdateRequest
{
    public string? Text { get; set; }
    public List<IFormFile>? Imgs { get; set; }  
    public List<int>? ImgIndex { get; set; }
}