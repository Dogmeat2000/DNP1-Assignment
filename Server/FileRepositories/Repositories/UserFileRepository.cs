using Entities;
using FileRepositories.Persistance;
using RepositoryContracts;

namespace FileRepositories.Repositories;

public class UserFileRepository : IUserRepository {
    private readonly string _filePath = Directory.GetCurrentDirectory() + @"\DataFiles\users.json";
    private IFilePersistance FileManager { get; } = new FilePersistance();
    private List<User> UserList { get; set; } = [];
    public string ErrorAddFailed { get; } = "Error occured while adding User. Data failed to load.";
    public string ErrorUpdateFailed { get; } = "Error occured while updating User. Data failed to load.";
    public string ErrorDeleteFailed { get; } = "Error occured while deleting User. Data failed to load.";
    public string ErrorGetSingleFailed { get; } = "Error occured while retrieving a single User. Data failed to load.";
    public string ErrorGetManyFailed { get; } = "Error occured while retrieving all User. Data failed to load.";

    
    public async Task<User> AddAsync(User user) {
                        
        // Load raw data from file:
        List<object>? rawData = await FileManager.ReadFromJsonFileAsync(_filePath, new User());
        
        // If data is not null, continue, else abort by throwing an exception!
        if (rawData != null) {
            
            // Cast the loaded data to the proper load format:
            UserList = rawData.OfType<User>().ToList();
            
            // Identify a unique User_id to assign to this User.
            user.User_id = UserList.Any() 
                ? UserList.Max(u => u.User_id) + 1 
                : 1;
            
            // Add the new User to the list of Users:
            UserList.Add(user);
            
            // Cast the modified data back to the proper save format, and attempt to save:
            if (await FileManager.SaveToJsonFileAsync(_filePath, UserList.Cast<object>().ToList())) {
                Console.WriteLine($": Added User with ID '{user.User_id}'");
            } else {
                Console.WriteLine($": ERROR DID NOT SAVE User with ID '{user.User_id}'");
            }

        } else {
            throw new Exception(ErrorAddFailed);
        }
        
        return user;
    }

    
    public async Task UpdateAsync(User user) {
                        
        // Load raw data from file:
        List<object>? rawData = await FileManager.ReadFromJsonFileAsync(_filePath, new User());
        
        // If data is not null, continue, else abort by throwing an exception!
        if (rawData != null) {
            
            // Cast the loaded data to the proper load format:
            UserList = rawData.OfType<User>().ToList();
            
            // Check if the Post to modify actually exists:
            User? existingUser = UserList.SingleOrDefault(u => u.User_id == user.User_id);
            if (existingUser is null) {
                throw new InvalidOperationException($"Post with ID '{user.User_id}' not found");
            }
            
            // If it does exist, Remove it from the list of Comments:
            UserList.Remove(existingUser);
            
            // Add the modified Post to the List
            UserList.Add(user);
            
            // Cast the modified data back to the proper save format, and attempt to save:
            if (await FileManager.SaveToJsonFileAsync(_filePath, UserList.Cast<object>().ToList())) {
                Console.WriteLine($": Modified Post with ID '{user.User_id}'");
                // TODO Update associated UserProfiles also! -> Should happen in business UI Logic, instead of here!
            } else {
                Console.WriteLine($": ERROR DID NOT Modify User with ID '{user.User_id}'");
            }
        } else {
            throw new Exception(ErrorUpdateFailed);
        }
    }

    
    public async Task DeleteAsync(int userId) {
                        
        // Load raw data from file:
        List<object>? rawData = await FileManager.ReadFromJsonFileAsync(_filePath, new User());
        
        // If data is not null, continue, else abort by throwing an exception!
        if (rawData != null) {
            
            // Cast the loaded data to the proper load format:
            UserList = rawData.OfType<User>().ToList();
            
            // Check if the User to remove actually exists:
            User? userToRemove = UserList.SingleOrDefault(u => u.User_id == userId);
            if (userToRemove is null) {
                throw new InvalidOperationException($"User with ID '{userId}' not found");
            }
            
            // If it does exist, Remove it from the list of Users:
            UserList.Remove(userToRemove);
            
            // Cast the modified data back to the proper save format, and attempt to save:
            if (await FileManager.SaveToJsonFileAsync(_filePath, UserList.Cast<object>().ToList())) {
                Console.WriteLine($": DELETED User with ID '{userId}'");
                // TODO Delete associated profiles also! -> Should happen in business UI Logic, instead of here!
            } else {
                Console.WriteLine($": ERROR DID NOT Delete User with ID '{userId}'");
            }
        } else {
            throw new Exception(ErrorDeleteFailed);
        }
    }

    
    public async Task<User> GetSingleAsync(int userId) {
                                
        // Load raw data from file:
        List<object>? rawData = await FileManager.ReadFromJsonFileAsync(_filePath, new User());
        
        // If data is not null, continue, else abort by throwing an exception!
        if (rawData != null) {
            
            // Cast the loaded data to the proper load format:
            UserList = rawData.OfType<User>().ToList();
            
            // Check if the User actually exists:
            User? userToReturn = UserList.SingleOrDefault(u => u.User_id == userId);
            if (userToReturn is null) {
                throw new InvalidOperationException($"User with ID '{userId}' not found");
            }
            
            // If it does exist, return it:
            return userToReturn;
        } 
        throw new Exception(ErrorGetSingleFailed);
    }

    
    public IQueryable<User> GetMany() {
        // Load raw data from file:
        List<object>? rawData = FileManager.ReadFromJsonFileAsync(_filePath, new User()).Result;
        
        // If data is not null, continue, else abort by throwing an exception!
        if (rawData != null) {
            
            // Cast the loaded data to the proper load format:
            UserList = rawData.OfType<User>().ToList();
            
            // Return the entire list:
            return UserList.AsQueryable();
        } 
        throw new Exception(ErrorGetManyFailed);
    }
}