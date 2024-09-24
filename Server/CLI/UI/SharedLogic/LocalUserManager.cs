using Entities;
using RepositoryContracts;

namespace ConsoleApp1.UI.SharedLogic;

public class LocalUserManager {

    public User? LocalUser { get; private set; } = null;
    public UserProfile? LocalUserProfile { get; private set; } = null;
    
    /** Pass, as an argument, the desired string to display to the user on the same line user input is taken from, in the console. */
    public async Task<string?> ReadUserInputAsync(string visCmd) {
        var inputReceived = false;
        string? userInput = null;
        while (!inputReceived) {
            Console.Write(visCmd);
            userInput = Console.ReadLine();

            if (userInput != null)
                inputReceived = true;
        }
        return userInput;
    }


    public async Task<bool> Login(IUserRepository userRepo, IUserProfileRepository profileRepo, CLISettings settings) {
        // Prompt User to Login with a username:
        string? username = null;
        while (username == null) {
            Console.ForegroundColor = settings.AppPromptTextColor;
            Console.WriteLine(": Enter 'abort' to cancel at any time.\n");
            Console.Write(": Please enter username: ");
            Console.ResetColor();
            username = await ReadUserInputAsync("");
            
            if(username.ToLower().Equals("abort"))
                return false;
            
            // Check if the given username exists:
            if (profileRepo.GetMany().SingleOrDefault(p => p.Username.Equals(username)) == null) {
                Console.ForegroundColor = settings.ErrorTextColor;
                Console.WriteLine("\n: ERROR Username '" + username + "' does not exist");
                Console.ResetColor();
                username = null;
            }
        }
        
        // Prompt User to enter a password:
        string? password = null;
        while (password == null) {
            Console.ForegroundColor = settings.AppPromptTextColor;
            Console.Write(": Please enter password: ");
            Console.ResetColor();
            password = await ReadUserInputAsync("");
            
            if(password.ToLower().Equals("abort"))
                return false;
            
            // Check if the given username exists:
            if (profileRepo.GetMany().SingleOrDefault(p => p.Username.Equals(username) && p.Password.Equals(password)) == null) {
                Console.ForegroundColor = settings.ErrorTextColor;
                Console.WriteLine($": Wrong password!");
                Console.ResetColor();
                password = null;
            } else {
                LocalUserProfile = profileRepo.GetMany().SingleOrDefault(p => p.Username.Equals(username) && p.Password.Equals(password));
                
                // Check that a corresponding User exists, and assign local User objects:
                LocalUser = await userRepo.GetSingleAsync(LocalUserProfile?.User_id ?? -1);

                if (LocalUser != null && LocalUserProfile != null && LocalUser.User_id == LocalUserProfile.User_id) {
                    return true;
                }
                LocalUser = null;
                LocalUserProfile = null;
            }
        }
        return false;
    }

    public bool Logout() {
        if (LocalUser == null && LocalUserProfile == null)
            return false;
        
        LocalUser = null;
        LocalUserProfile = null;
        return true;
    }
}