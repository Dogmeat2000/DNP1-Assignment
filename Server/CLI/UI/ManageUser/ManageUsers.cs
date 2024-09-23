using ConsoleApp1.UI.SharedLogic;
using Entities;
using RepositoryContracts;

namespace ConsoleApp1.UI.ManageUser;

public class ManageUsers {
    IUserRepository UserRepo { get; set; }
    IUserProfileRepository UserProfileRepo { get; set; }
    private User? LocalUser { get; set; }
    private string errorMessage { get; set; }
    private bool invalidEntry  { get; set; }
    private bool returnToLastView { get; set; }

    public ManageUsers(IUserRepository userRepo, IUserProfileRepository userProfileRepo, User? localUser) {
        UserRepo = userRepo;
        UserProfileRepo = userProfileRepo;
        LocalUser = localUser;
        errorMessage = "ERROR: Please enter a valid command...!";
        invalidEntry = false;
        returnToLastView = false;
    }
    
    /** True = Method finished, and application should revert to Main Menu! */
    public async Task<bool> Start() {
                
        // User UI Logic is encapsulated inside this repeating while-loop:
        while (!returnToLastView) {
            
            // Display Main Menu Text:
            ShowUserMenuText();
            
            // Display last error, if user previously entered invalid command:
            if (invalidEntry) {
                Console.WriteLine(errorMessage);
                invalidEntry = false;
            }
                
            // Read userInput, and pick a corresponding command
            EvaluateCommand(await new LocalUserManager().ReadUserInputAsync("\nCmd: ") ?? "INVALID COMMAND");
        }
        return true;
    }
    
    private void ShowUserMenuText() {
        
        if(LocalUser != null) {
            var userProfile = UserProfileRepo.GetMany().FirstOrDefault(user => user.User_id == LocalUser.User_id); //TODO Get the users name here!
            Console.WriteLine($"\n\n\nLogged in as: {userProfile.Username}");
        } else {
            var userName = "Anonymous";
            Console.WriteLine($"\n\n\nViewing as: {userName}");
        }
        
        Console.WriteLine("|--------------------------------------------------------------------------------------------------------------------------------|");
        Console.WriteLine("|                                                            \x1b[1mUser Menu:\x1b[0m                                                          |");
        Console.WriteLine("|--------------------------------------------------------------------------------------------------------------------------------|");
        
        // Display operations only available for anonymous users:
        if (LocalUser != null) {
            // Display operations only available for logged-in users:
            Console.WriteLine("| Type 'update' to: Update an existing user                                                                                      |");
            Console.WriteLine("| Type 'list' to: Print a list of all users                                                                                      |");
            Console.WriteLine("| Type 'delete' to: Delete a User                                                                                                |");
        } else {
            // Display operations only available for anonymous users:
            Console.WriteLine("| Type 'login' to: Login to your account                                                                                         |");
        }
        
        // Display operations available to all users:
        Console.WriteLine("| Type 'new' to: Create a new user                                                                                               |");
        Console.WriteLine("| Type 'return' to: Return to Main Menu                                                                                          |");
        Console.WriteLine("|--------------------------------------------------------------------------------------------------------------------------------|");
    }
    
    private void EvaluateCommand(string cmd) {
        switch (cmd.ToLower()) {
            case "update":
                if (LocalUser != null) {
                    //TODO Update User
                } else {
                    Console.WriteLine(errorMessage);
                    invalidEntry = true;
                }
                break;
            
            case "list":
                if (LocalUser != null) {
                    //TODO List all users
                } else {
                    Console.WriteLine(errorMessage);
                    invalidEntry = true;
                }
                break;
            
            case "delete":
                if (LocalUser != null) {
                    //TODO Delete a user
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
            
            case "new": 
                //TODO Create new user!
                break;
            
            case "return": 
                returnToLastView = true;
                break;
            
            default:
                Console.WriteLine(errorMessage);
                invalidEntry = true;
                break;
        }
    }
}