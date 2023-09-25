namespace Backend.DataBase.Request;

public class UserRequest
{
    public string Login { get; set; }
    public string Password { get; set; }
    public string? Mail { get; set; }
}