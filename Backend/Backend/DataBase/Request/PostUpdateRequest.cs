namespace Backend.DataBase.Request;

public class PostUpdateRequest
{
    public int? IdDeveloper { get; set; }
    public string? Title { get; set; }
    public string? Text { get; set; }
    public List<IFormFile>? Imgs { get; set; }  
    public List<int> ImgIndexToUpdate { get; set; }
    
}