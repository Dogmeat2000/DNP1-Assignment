using ConsoleApp1.UI.SharedLogic;
using Entities;
using RepositoryContracts;

namespace ConsoleApp1.UI.ManageUser;

public class CreateUser {
    private bool ReturnToLastView { get; set; }
    private string? UserName { get; set; }
    private string? Password { get; set; }

    public CreateUser() {
        UserName = "";
        Password = "";
    }

    public async Task<UserProfile?> NewUserAsync(IUserRepository userRepo, IUserProfileRepository userProfileRepo) {
        User? newUser = null;
        UserProfile? newUserProfile = null;
        
        // User UI Logic is encapsulated inside this repeating while-loop:
        while (!ReturnToLastView) {
            Console.WriteLine("\n-> Creating a new user! [type 'return' to abort]");

            // Let user select a username:
            if (!SelectUserNameAsync().Result)
                return newUserProfile;
            
            // Let user select a password:
            if (!SelectPassword().Result)
                return newUserProfile;
           
            // Create the new user:
            newUser = await userRepo.AddAsync(new User(-1));
            
            //TODO Check that user was created (is not null)!
            
            // Create and add a corresponding userProfile
            newUserProfile = await userProfileRepo.AddAsync(new UserProfile(-1, newUser.User_id));
            newUserProfile.Username = UserName ?? "ERROR NO NAME!";
            newUserProfile.Password = Password ?? "ERROR NO PASSWORD!";

            await userProfileRepo.UpdateAsync(newUserProfile);
            
            //TODO Check that the profile was created. If not, delete the created user!
            
            ReturnToLastView = true;
        }
        return newUserProfile;
    }
    
    private async Task<bool> SelectPassword() {
        while (string.IsNullOrEmpty(Password)) {
            Console.Write("Please enter a Password: ");
            var password = await new UserInput().ReadUserInputAsync_Alt1() ?? "";

            if (password.ToLower() == "abort")
                return false;
                
            if(ValidatePassword(password))
                Password = password;
        }
        return true;
    }

    private async Task<bool> SelectUserNameAsync() {
        while (string.IsNullOrEmpty(UserName)) {
            Console.Write("Please enter a Username: ");
            var username = await new UserInput().ReadUserInputAsync_Alt1() ?? "";

            if (username.ToLower() == "abort")
                return false;
                
            if(ValidateUserName(username))
                UserName = username;
        }
        return true;
    }

    private bool ValidateUserName(string userName) {
        // TODO Implement validation logic
        return true;
    }

    private bool ValidatePassword(string password) {
        // TODO Implement validation logic
        return true;
    }
}