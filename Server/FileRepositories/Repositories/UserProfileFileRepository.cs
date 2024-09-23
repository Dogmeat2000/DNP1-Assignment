using Entities;
using FileRepositories.Persistance;
using RepositoryContracts;

namespace FileRepositories.Repositories;

public class UserProfileFileRepository : IUserProfileRepository {
    private readonly string _filePath = Directory.GetCurrentDirectory() + @"\DataFiles\userprofiles.json";
    private IFilePersistance FileManager { get; } = new FilePersistance();
    private List<UserProfile> UserProfileList { get; set; } = [];
    public string ErrorAddFailed { get; } = "Error occured while adding UserProfile. Data failed to load.";
    public string ErrorUpdateFailed { get; } = "Error occured while updating UserProfile. Data failed to load.";
    public string ErrorDeleteFailed { get; } = "Error occured while deleting UserProfile. Data failed to load.";
    public string ErrorGetSingleFailed { get; } = "Error occured while retrieving a single UserProfile. Data failed to load.";
    public string ErrorGetManyFailed { get; } = "Error occured while retrieving all UserProfiles. Data failed to load.";

    
    public async Task<UserProfile> AddAsync(UserProfile userProfile) {
                                
        // Load raw data from file:
        List<object>? rawData = await FileManager.ReadFromJsonFileAsync(_filePath, new UserProfile());
        
        // If data is not null, continue, else abort by throwing an exception!
        if (rawData != null) {
            
            // Cast the loaded data to the proper load format:
            UserProfileList = rawData.OfType<UserProfile>().ToList();
            
            // Identify a unique Profile_id to assign to this User.
            userProfile.Profile_id = UserProfileList.Any() 
                ? UserProfileList.Max(uP => uP.Profile_id) + 1 
                : 1;
            
            // Add the new User to the list of Users:
            UserProfileList.Add(userProfile);
            
            // Cast the modified data back to the proper save format, and attempt to save:
            if (await FileManager.SaveToJsonFileAsync(_filePath, UserProfileList.Cast<object>().ToList())) {
                Console.WriteLine($": Added UserProfile with ID '{userProfile.Profile_id}' to User with ID '{userProfile.User_id}'");
            } else {
                Console.WriteLine($": ERROR DID NOT SAVE UserProfile with ID '{userProfile.Profile_id}' to User with ID '{userProfile.User_id}'");
            }

        } else {
            throw new Exception(ErrorAddFailed);
        }
        
        return userProfile;
    }

    
    public async Task UpdateAsync(UserProfile userProfile) {
                                
        // Load raw data from file:
        List<object>? rawData = await FileManager.ReadFromJsonFileAsync(_filePath, new UserProfile());
        
        // If data is not null, continue, else abort by throwing an exception!
        if (rawData != null) {
            
            // Cast the loaded data to the proper load format:
            UserProfileList = rawData.OfType<UserProfile>().ToList();
            
            // Check if the Post to modify actually exists:
            UserProfile? existingUserProfile = UserProfileList.SingleOrDefault(uP => uP.Profile_id == userProfile.Profile_id && uP.User_id == userProfile.User_id);
            if (existingUserProfile is null) {
                throw new InvalidOperationException($"UserProfile with ID '{userProfile.Profile_id}' for User with ID {userProfile.User_id} not found");
            }
            
            // If it does exist, Remove it from the list of Comments:
            UserProfileList.Remove(existingUserProfile);
            
            // Add the modified Post to the List
            UserProfileList.Add(userProfile);
            
            // Cast the modified data back to the proper save format, and attempt to save:
            if (await FileManager.SaveToJsonFileAsync(_filePath, UserProfileList.Cast<object>().ToList())) {
                Console.WriteLine($": Modified UserProfile with ID '{userProfile.Profile_id}' associated with User with ID '{userProfile.User_id}'");
            } else {
                Console.WriteLine($": ERROR DID NOT Modify UserProfile with ID '{userProfile.Profile_id}' associated with User with ID '{userProfile.User_id}'");
            }
        } else {
            throw new Exception(ErrorUpdateFailed);
        }
    }

    
    public async Task DeleteAsync(int profileId, int userId) {
                                
        // Load raw data from file:
        List<object>? rawData = await FileManager.ReadFromJsonFileAsync(_filePath, new UserProfile());
        
        // If data is not null, continue, else abort by throwing an exception!
        if (rawData != null) {
            
            // Cast the loaded data to the proper load format:
            UserProfileList = rawData.OfType<UserProfile>().ToList();
            
            // Check if the UserProfile to remove actually exists:
            UserProfile? userProfileToRemove = UserProfileList.SingleOrDefault(uP => uP.Profile_id == profileId  && uP.User_id == userId);
            if (userProfileToRemove is null) {
                throw new InvalidOperationException($"UserProfile with ID '{profileId}' associated with User with ID '{userId}' not found");
            }
            
            // If it does exist, Remove it from the list of UserProfiles:
            UserProfileList.Remove(userProfileToRemove);
            
            // Cast the modified data back to the proper save format, and attempt to save:
            if (await FileManager.SaveToJsonFileAsync(_filePath, UserProfileList.Cast<object>().ToList())) {
                Console.WriteLine($": DELETED UserProfile with ID '{profileId}' associated with User with ID '{userId}'");
            } else {
                Console.WriteLine($": ERROR DID NOT Delete UserProfile with ID '{profileId}' associated with User with ID '{userId}'");
            }
        } else {
            throw new Exception(ErrorDeleteFailed);
        }
    }

    
    public async Task<UserProfile> GetSingleAsync(int profileId, int userId) {
                                        
        // Load raw data from file:
        List<object>? rawData = await FileManager.ReadFromJsonFileAsync(_filePath, new UserProfile());
        
        // If data is not null, continue, else abort by throwing an exception!
        if (rawData != null) {
            
            // Cast the loaded data to the proper load format:
            UserProfileList = rawData.OfType<UserProfile>().ToList();
            
            // Check if the UserProfile actually exists:
            UserProfile? userProfileToReturn = UserProfileList.SingleOrDefault(uP => uP.Profile_id == profileId && uP.User_id == userId);
            if (userProfileToReturn is null) {
                throw new InvalidOperationException($"UserProfile with ID '{profileId}' associated with User with ID '{userId}' not found");
            }
            
            // If it does exist, return it:
            return userProfileToReturn;
        } 
        throw new Exception(ErrorGetSingleFailed);
    }

    
    public IQueryable<UserProfile> GetMany() {
        // Load raw data from file:
        List<object>? rawData = FileManager.ReadFromJsonFileAsync(_filePath, new UserProfile()).Result;
        
        // If data is not null, continue, else abort by throwing an exception!
        if (rawData != null) {
            
            // Cast the loaded data to the proper load format:
            UserProfileList = rawData.OfType<UserProfile>().ToList();
            
            // Return the entire list:
            return UserProfileList.AsQueryable();
        } 
        throw new Exception(ErrorGetManyFailed);
    }
}