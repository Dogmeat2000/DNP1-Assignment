using ApiContracts;
using Entities;

namespace DTOconverters;

public class UserProfileConverter {
    public static UserProfileDTO UserProfileToDTO(UserProfile userProfile) {
        return new UserProfileDTO() {
            Profile_id = userProfile.Profile_id,
            Username = userProfile.Username,
            Password = userProfile.Password,
            User_id = userProfile.User_id,
        };
    }
    
    public static UserProfile DTOToUserProfile(UserProfileDTO dtoObject) {
        return new UserProfile() {
            Profile_id = dtoObject.Profile_id,
            Username = dtoObject.Username,
            Password = dtoObject.Password,
            User_id = dtoObject.User_id,
        };
    }
}