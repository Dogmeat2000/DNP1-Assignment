namespace Entities;

public class UserProfile
{
    public int Profile_id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public int User_id { get; set; }

    public UserProfile() {
        Username = "";
        Password = "";
        //TODO
    }
}