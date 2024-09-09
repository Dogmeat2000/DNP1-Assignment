using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class UserProfileInMemoryRepository : IUserProfileRepository {
    
    public List<UserProfile> UserProfileList;

    public UserProfileInMemoryRepository() {
        UserProfileList = [];
        GenerateDummyData();
    }

    public Task<UserProfile> AddAsync(UserProfile userProfile) {
        userProfile.User_id = UserProfileList.Any() 
            ? UserProfileList.Max(uP => uP.User_id) + 1
            : 1;
        UserProfileList.Add(userProfile);
        return Task.FromResult(userProfile);
    }

    public Task UpdateAsync(UserProfile userProfile) {
        UserProfile ? existingUser = UserProfileList.SingleOrDefault(uP => uP.Profile_id == userProfile.Profile_id && uP.User_id == userProfile.User_id);
        if (existingUser is null) {
            throw new InvalidOperationException(
                $"UserProfile with ID '{userProfile.Profile_id}' for User '{userProfile.User_id}' not found");
        }

        UserProfileList.Remove(existingUser);
        UserProfileList.Add(userProfile);

        return Task.CompletedTask;
    }

    public Task DeleteAsync(int profileId, int userId) {
        UserProfile ? userProfileToRemove = UserProfileList.SingleOrDefault(uP => uP.Profile_id == profileId && uP.User_id == userId);
        if (userProfileToRemove is null) {
            throw new InvalidOperationException(
                $"UserProfile with ID '{profileId}' for User '{userId}' not found");
        }

        UserProfileList.Remove(userProfileToRemove);
        return Task.CompletedTask;
    }

    public Task<UserProfile> GetSingleAsync(int profileId, int userId) {
        UserProfile ? userProfileToReturn = UserProfileList.SingleOrDefault(uP => uP.Profile_id == profileId && uP.User_id == userId);
        if (userProfileToReturn is null) {
            throw new InvalidOperationException(
                $"UserProfile with ID '{profileId}' for User '{userId}' not found");
        }
        
        return Task.FromResult(userProfileToReturn);
    }

    public IQueryable<UserProfile> GetMany() {
        return UserProfileList.AsQueryable();
    }
    
    private void GenerateDummyData() {
        for (int i = 0; i <= 5; i++) {
            UserProfile userProfile = new UserProfile();
            switch (i) {
                case 0:
                    userProfile.Profile_id = 5;
                    userProfile.Username = "Test User 1";
                    userProfile.Password = "A1234-qwerty";
                    userProfile.User_id = i;
                    break;
                
                case 1: 
                    userProfile.Profile_id = 4;
                    userProfile.Username = "Test User 2";
                    userProfile.Password = "A1234-qwerty";
                    userProfile.User_id = i;
                    break;
                
                case 2: 
                    userProfile.Profile_id = 3;
                    userProfile.Username = "Test User 3";
                    userProfile.Password = "A1234-qwerty";
                    userProfile.User_id = i;
                    break;
                
                case 3: 
                    userProfile.Profile_id = 2;
                    userProfile.Username = "Test User 4";
                    userProfile.Password = "A1234-qwerty";
                    userProfile.User_id = i;
                    break;
                
                case 4: 
                    userProfile.Profile_id = 1;
                    userProfile.Username = "Test User 5";
                    userProfile.Password = "A1234-qwerty";
                    userProfile.User_id = i;
                    break;
                
                case 5: 
                    userProfile.Profile_id = 0;
                    userProfile.Username = "Test User 6";
                    userProfile.Password = "A1234-qwerty";
                    userProfile.User_id = i;
                    break;
                
                default: 
                    break;
            }
            UserProfileList.Add(userProfile);
        }
    }
}