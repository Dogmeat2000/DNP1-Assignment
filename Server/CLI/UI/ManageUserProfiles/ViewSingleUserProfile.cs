using Entities;
using RepositoryContracts;

namespace ConsoleApp1.UI.ManageUserProfiles;

public class ViewSingleUserProfile {

    public UserProfile? LookUpUserProfileAsync(User user, IUserProfileRepository userProfileRepo) {
        
        return userProfileRepo.GetMany().First(u => u.User_id == user.User_id);
    }
}