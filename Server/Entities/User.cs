using System.ComponentModel.DataAnnotations;

namespace Entities;

public class User
{
    public int User_id { get; set; }
    
    // Navigation Properties:
    public UserProfile? UserProfile { get; set; } = null;
    public List<Forum> ManagedForums { get; } = new();
    public List<Post> ManagedPosts { get; } = new();
    public List<Comment> ManagedComments { get; } = new();
    
    
    public User(int user_id) {
        User_id = user_id;
    }
    
    // Required by EFC:
    public User() {}
}