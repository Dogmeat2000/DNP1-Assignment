using ApiContracts;
using Entities;

namespace DTOconverters;

public class UserConverter {
    public static UserDTO UserToDTO(User user) {
        return new UserDTO() {
            User_id = user.User_id,
        };
    }
    
    public static User DTOToUser(UserDTO dtoObject) {
        return new User() {
            User_id = dtoObject.User_id,
        };
    }
}