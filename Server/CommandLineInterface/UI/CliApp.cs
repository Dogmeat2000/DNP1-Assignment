using ConsoleApp1.UI.ManageUser;
using Entities;
using RepositoryContracts;

namespace ConsoleApp1.UI;

public class CliApp {
    
    public ICommentRepository CommentRepository { get; set; }
    public IForumRepository ForumRepository { get; set; }
    public IPostRepository PostRepository { get; set; }
    public IUserRepository UserRepository { get; set; }
    public IUserProfileRepository UserProfileRepository { get; set; }
    private User? LocalUser { get; set; }
    private string errorMessage { get; set; }
    private Boolean invalidEntry  { get; set; }

    public CliApp(ICommentRepository commentRepository, IForumRepository forumRepository, IPostRepository postRepository, IUserRepository userRepository, IUserProfileRepository userProfileRepository) {
        this.CommentRepository = commentRepository;
        this.ForumRepository = forumRepository;
        this.PostRepository = postRepository;
        this.UserRepository = userRepository;
        this.UserProfileRepository = userProfileRepository;
        this.LocalUser = null;
        errorMessage = "ERROR: Please enter a valid command...!";
        invalidEntry = false;
    }

    public async Task StartAsync() {
        
        // Main UI Logic is encapsulated inside this repeating while-loop:
        var runApp = true;
        
        while (runApp) {
            
            // Display Main Menu Text:
            ShowMainMenuText();
            
            // TODO Display top level forums!
            
            // Display last error, if user previously entered invalid command:
            if (invalidEntry) {
                Console.WriteLine(errorMessage);
                invalidEntry = false;
            }
                
            // Read userInput, and pick a corresponding command
            runApp = EvaluateCommand(ReadUserInput());
        }
    }

    private void ShowMainMenuText() {
        var userName = "Anonymous";
        if(LocalUser != null) {
            userName = "Anonymous"; //TODO Get the users name here!
            Console.WriteLine($"\n\n\nLogged in as: {userName}");
        } else {
            Console.WriteLine($"\n\n\nViewing as: {userName}");
        }
        Console.WriteLine("|--------------------------------------------------------------------------------------------------------------------------------|");
        Console.WriteLine("|                                                            \x1b[1mMain Menu:\x1b[0m                                                          |");
        Console.WriteLine("|--------------------------------------------------------------------------------------------------------------------------------|");
        
        // Display operations only available for anonymous users
        if (LocalUser != null) {
            // Display operations only available for logged-in users
            Console.WriteLine("| Type 'user' to: Manage user (Create, View, Update or Delete)                                                                   |");
            Console.WriteLine("| Type 'profile' to: Manage your own user profile (change password, name, etc.)                                                  |");
            Console.WriteLine("| Type 'logout' to: Logout                                                                                                       |");
        } else {
            // Display operations only available for anonymous users
            Console.WriteLine("| Type 'login' to: Login to your account                                                                                         |");
            Console.WriteLine("| Type 'create' to: Create a User Account                                                                                        |");
        }
        Console.WriteLine("| Type 'cd ' + 'identifier' next to forum or post names in order to view them. Ex: 'cd F1' to view/enter Forum 1                 |");
        Console.WriteLine("| Type 'exit' to: Terminate this application                                                                                     |");
        Console.WriteLine("|--------------------------------------------------------------------------------------------------------------------------------|");
    }

    private string ReadUserInput() {
        var inputReceived = false;
        string? userInput = null;
        while (!inputReceived) {
            Console.Write("\nCmd: ");
            userInput = Console.ReadLine();

            if (userInput != null) {
                inputReceived = true;
                
            }
        }
        return userInput;
    }

    private Boolean EvaluateCommand(string cmd) {
        switch (cmd.ToLower()) {
            case "user":
                if (LocalUser != null) {
                    new ManageUsers(UserRepository, UserProfileRepository, LocalUser).Start();
                } else {
                    Console.WriteLine(errorMessage);
                    invalidEntry = true;
                }
                break;
            
            case "profile":
                if (LocalUser != null) {
                    //TODO Go to userProfile view!
                } else {
                    Console.WriteLine(errorMessage);
                    invalidEntry = true;
                }
                break;
            
            case "logout":
                if (LocalUser != null) {
                    //TODO Go to user logout view!
                } else {
                    Console.WriteLine(errorMessage);
                    invalidEntry = true;
                }
                break;
            
            case "login":
                if (LocalUser != null) {
                    Console.WriteLine(errorMessage);
                    invalidEntry = true;
                } else {
                    //TODO Go to user login view!
                }
                break;
            
            case "create":
                if (LocalUser != null) {
                    Console.WriteLine(errorMessage);
                    invalidEntry = true;
                } else {
                    new ManageUsers(UserRepository, UserProfileRepository, LocalUser).CreateUser();
                }
                break;
            
            case "exit":
                Console.WriteLine("Terminating application...!");
                return false;
            
            default:
                Console.WriteLine(errorMessage);
                invalidEntry = true;
                break;
        }
        return true;
    }
}