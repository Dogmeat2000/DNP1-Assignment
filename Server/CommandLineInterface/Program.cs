using Entities;
using InMemoryRepositories;

namespace ConsoleApp1;

class Program {
    static void Main(string[] args) {
                
        Console.WriteLine("\n\nLaunching Forum Backend (Server)");
        
        // Initialize InMemoryRepositories
        Console.Write("Initializing 'InMemoryRepositories'...");
        
        CommentInMemoryRepository commentManager = new CommentInMemoryRepository();
        ForumInMemoryRepository forumManager = new ForumInMemoryRepository();
        PostInMemoryRepository postManager = new PostInMemoryRepository();
        UserInMemoryRepository userManager = new UserInMemoryRepository();
        UserProfileInMemoryRepository userProfileManager = new UserProfileInMemoryRepository();

        Console.WriteLine(" DONE!");
        
        // Test if data exists in repos
        Console.WriteLine("\nReading generated data from 'CommentInMemoryRepository'...");
        foreach (Comment comment in commentManager.GetMany()) {
            Console.WriteLine($"Comment_id='{comment.Comment_id}': exists");
        }
        
        Console.WriteLine("\nReading generated data from 'ForumInMemoryRepository'...");
        foreach (Forum forum in forumManager.GetMany()) {
            Console.WriteLine($"Forum_id='{forum.Forum_id}': exists");
        }
        
        Console.WriteLine("\nReading generated data from 'PostInMemoryRepository'...");
        foreach (Post post in postManager.GetMany()) {
            Console.WriteLine($"Post_id='{post.Post_id}': exists");
        }
        
        Console.WriteLine("\nReading generated data from 'UserInMemoryRepository'...");
        foreach (User user in userManager.GetMany()) {
            Console.WriteLine($"user_id='{user.User_id}': exists");
        }
        
        Console.WriteLine("\nReading generated data from 'UserProfileInMemoryRepository'...");
        foreach (UserProfile userProfile in userProfileManager.GetMany()) {
            Console.WriteLine($"Profile_id='{userProfile.Profile_id}': exists");
        }
        
        Console.WriteLine("\n\nFORUM BACKEND IS (mostly...) READY!");
    }
}