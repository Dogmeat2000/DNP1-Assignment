using ConsoleApp1.UI;
using Entities;
using FileRepositories.Repositories;
using InMemoryRepositories;
using RepositoryContracts;

namespace ConsoleApp1;

class Program {
    static async Task Main(string[] args) {
        // Initialize backend.
        Console.WriteLine("\nLaunching Forum Backend (Server)");
        
        // Initialize InMemoryRepositories
        Console.Write("Initializing 'InMemoryRepositories'...");
        
        // Old InMemoryRepositories commented out and replaced by FileRepositories:
        /*ICommentRepository commentManager = new CommentInMemoryRepository();
        IForumRepository forumManager = new ForumInMemoryRepository();
        IPostRepository postManager = new PostInMemoryRepository();
        IUserRepository userManager = new UserInMemoryRepository();*/
        IUserProfileRepository userProfileManager = new UserProfileInMemoryRepository();
        Console.WriteLine(" DONE!");
        
        // Current FileRepositories:
        ICommentRepository commentManager = new CommentFileRepository();
        IForumRepository forumManager = new ForumFileRepository();
        IPostRepository postManager = new PostFileRepository();
        IUserRepository userManager = new UserFileRepository();
        
        // Test if data exists in repos
        Console.WriteLine("\nChecking data integrity...");
        var count = 0;
        foreach (Comment comment in commentManager.GetMany()) {
            count++;
        }
        Console.WriteLine($"'CommentInMemoryRepository' contains {count} entries");
        
        count = 0;
        foreach (Forum forum in forumManager.GetMany()) {
            count++;
        }
        Console.WriteLine($"'ForumInMemoryRepository' contains {count} entries");
        
        count = 0;
        foreach (Post post in postManager.GetMany()) {
            count++;
        }
        Console.WriteLine($"'PostInMemoryRepository' contains {count} entries");
        
        count = 0;
        foreach (User user in userManager.GetMany()) {
            count++;
        }
        Console.WriteLine($"'UserInMemoryRepository' contains {count} entries");
        
        count = 0;
        foreach (UserProfile userProfile in userProfileManager.GetMany()) {
            count++;
        }
        Console.WriteLine($"'UserProfileInMemoryRepository' contains {count} entries");
        
        Console.WriteLine("\nFORUM BACKEND IS READY!");
        
        // Initialize User Interface
        Console.WriteLine("\n\nInitializing Command Line Interface...");
        CliApp cliApp = new CliApp(commentManager, forumManager, postManager, userManager, userProfileManager);
        await cliApp.StartAsync();
    }
}