namespace Entities;

public class UserProfile
{
    public int Profile_id { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
    public int User_id { get; set; }

    // Navigation Properties:
    public User User { get; set; } = null!;
    
    
    
    
    
    
    
    public UserProfile(int profile_id, int user_id) {
        Username = "";
        Password = "";
        Profile_id = profile_id;
        User_id = user_id;
    }
    
    // Required by EFC:
    public UserProfile() {}
}