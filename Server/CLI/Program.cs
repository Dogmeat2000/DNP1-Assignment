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
        IUserRepository userManager = new UserInMemoryRepository();
        IUserProfileRepository userProfileManager = new UserProfileInMemoryRepository();*/
        
        // Current FileRepositories:
        ICommentRepository commentManager = new CommentFileRepository();
        IForumRepository forumManager = new ForumFileRepository();
        IPostRepository postManager = new PostFileRepository();
        IUserRepository userManager = new UserFileRepository();
        IUserProfileRepository userProfileManager = new UserProfileFileRepository();
        Console.WriteLine(" DONE!");
        
        // Test if data exists in repos
        Console.WriteLine("\nChecking data integrity...");
        // CheckDataIntegrityInMemoryRepositories(commentManager, forumManager, postManager, userManager, userProfileManager);
        CheckDataIntegrityInFileRepositories(commentManager, forumManager, postManager, userManager, userProfileManager);
        Console.WriteLine("\nIntegrity Check completed");
        
        // Initialize User Interface
        Console.WriteLine("\n\nInitializing Command Line Interface...");
        CliApp cliApp = new CliApp(commentManager, forumManager, postManager, userManager, userProfileManager);
        await cliApp.StartAsync();
    }

    
    private static void CheckDataIntegrityInFileRepositories(ICommentRepository commentManager, IForumRepository forumManager, IPostRepository postManager, IUserRepository userManager, IUserProfileRepository userProfileManager) {
        var count = 0;
        try {
            foreach (Comment comment in commentManager.GetMany()) {
                count++;
            }

            Console.WriteLine($"'CommentFileRepository' contains {count} entries");
        } catch (Exception e) {
            if (e.Message.Equals("Error occured while retrieving all comments. Data failed to load."))
                Console.WriteLine("WARNING: NO COMMENTS EXIST IN REPOSITORY! Might indicate loss of data integrity, or that no Comments have ever been made!");
        }

        try {
            count = 0;
            foreach (Forum forum in forumManager.GetMany()) {
                count++;
            }

            Console.WriteLine($"'ForumFileRepository' contains {count} entries");
        } catch (Exception e) {
            if (e.Message.Equals("Error occured while retrieving all Forums. Data failed to load."))
                Console.WriteLine("WARNING: NO FORUMS EXIST IN REPOSITORY! Might indicate loss of data integrity, or that no Forums have ever been made!");
        }

        try {
            count = 0;
            foreach (Post post in postManager.GetMany()) {
                count++;
            }
            Console.WriteLine($"'PostInFileRepository' contains {count} entries");
        } catch (Exception e) {
            if (e.Message.Equals("Error occured while retrieving all Posts. Data failed to load."))
                Console.WriteLine("WARNING: NO POSTS EXIST IN REPOSITORY! Might indicate loss of data integrity, or that no Posts have ever been made!");
        }

        try {
            count = 0;
            foreach (User user in userManager.GetMany()) {
                count++;
            }
            Console.WriteLine($"'UserInFileRepository' contains {count} entries");
        } catch (Exception e) {
            if (e.Message.Equals("Error occured while retrieving all Users. Data failed to load."))
                Console.WriteLine("WARNING: NO USERS EXIST IN REPOSITORY! Might indicate loss of data integrity, or that no Users have ever been made!");
        }

        try {
            count = 0;
            foreach (UserProfile userProfile in userProfileManager.GetMany()) {
                count++;
            }
            Console.WriteLine($"'UserProfileInFileRepository' contains {count} entries");
        } catch (Exception e) {
            if (e.Message.Equals("Error occured while retrieving all UserProfiles. Data failed to load."))
                Console.WriteLine("WARNING: NO USERPROFILES EXIST IN REPOSITORY! Might indicate loss of data integrity, or that no UserProfiles have ever been made!");
        }
    }

    
    private static void CheckDataIntegrityInMemoryRepositories(ICommentRepository commentManager, IForumRepository forumManager, IPostRepository postManager, IUserRepository userManager, IUserProfileRepository userProfileManager) {
        var count = 0;
        foreach (Comment comment in commentManager.GetMany()) {
            count++;
        }

        Console.WriteLine($"'CommentFileRepository' contains {count} entries");

        count = 0;
        foreach (Forum forum in forumManager.GetMany()) {
            count++;
        }

        Console.WriteLine($"'ForumFileRepository' contains {count} entries");

        
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
    }
}