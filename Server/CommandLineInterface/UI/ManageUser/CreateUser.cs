using Entities;
using RepositoryContracts;

namespace ConsoleApp1.UI.ManageUser;

public class CreateUser {
    IUserRepository UserRepo { get; set; }
    IUserProfileRepository UserProfileRepo { get; set; }
    private string UserName { get; set; }
    private string Password { get; set; }
    private bool returnToLastView { get; set; }

    public CreateUser(IUserRepository userRepo, IUserProfileRepository userProfileRepo) {
        this.UserRepo = userRepo;
        this.UserProfileRepo = userProfileRepo;
        UserName = "";
        Password = "";
    }
    
    /** False = action aborted. True = action successful */
    public bool Start() {
        // User UI Logic is encapsulated inside this repeating while-loop:
        while (!returnToLastView) {
            Console.WriteLine("-> Creating a new user! [type 'return' to abort]");

            // Let user select a username:
            if (!SelectUserName())
                return false;
            
            // Let user select a password:
            if (!SelectPassword())
                return false;
           
            // Create the new user:
            var newUser = new User(GenerateUserId());
            UserRepo.AddAsync(newUser);
            
            // Create and add a corresponding userProfile
            var newProfile = new UserProfile(GenerateProfileId(), newUser.User_id) {
                Username = UserName,
                Password = Password
            };
            UserProfileRepo.AddAsync(newProfile);
            Console.WriteLine($"-> New user created [name = {UserName}, password = {Password}]!");
  
            returnToLastView = true;
        }
        return true;
    }

    private bool SelectPassword() {
        while (Password.Length == 0) {
            Console.Write("Please enter a Password: ");
            var password = ReadUserInput();

            if (password.ToLower() == "abort")
                return false;
                
            if(ValidatePassword(password))
                Password = password;
        }
        return true;
    }

    private bool SelectUserName() {
        while (UserName.Length == 0) {
            Console.Write("Please enter a Username: ");
            var name = ReadUserInput();

            if (name.ToLower() == "abort")
                return false;
                
            if(ValidateUserName(name))
                UserName = name;
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

    private int GenerateUserId() {
        // Determine user_id to assign:
        var newUserId = UserRepo.GetMany().Count();
        var userIdValidated = false;
        while (!userIdValidated) {
            newUserId++;
            userIdValidated = true;
                
            foreach (var user in UserRepo.GetMany()) {
                if (user.User_id == newUserId) {
                    userIdValidated = false;
                    break;
                }
            }
        }
        return newUserId;
    }

    private int GenerateProfileId() {
        // Determine profile_id to assign:
        var newProfileId = UserProfileRepo.GetMany().Count();
        var userProfileIdValidated = false;
        while (!userProfileIdValidated) {
            newProfileId++;
            userProfileIdValidated = true;
                
            foreach (var userProfile in UserProfileRepo.GetMany()) {
                if (userProfile.Profile_id == newProfileId) {
                    userProfileIdValidated = false;
                    break;
                }
            }
        }
        return newProfileId;
    }
    
    private string ReadUserInput() {
        var inputReceived = false;
        string? userInput = null;
        while (!inputReceived) {
            userInput = Console.ReadLine();

            if (userInput != null) {
                inputReceived = true;
                
            }
        }
        return userInput;
    }
}