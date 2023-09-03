namespace Backend.DataBase.Request;

public class UpdateUserRequest
{
    public string Login { get; set; }
    public string? Mail { get; set; }
    public IFormFile? Avatar { get; set; } = null;
    public string? Role { get; set; } = "developer";
}