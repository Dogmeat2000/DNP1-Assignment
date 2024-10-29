using ApiContracts;

namespace ClientApp.Services;

public interface IUserProfileService {
    Task<UserProfileDTO> AddUserProfileAsync(UserProfileDTO request);
    Task UpdateUserProfileAsync(int id, UserProfileDTO request);
    Task DeleteUserProfileAsync(int userId, UserProfileDTO request);
    Task<UserProfileDTO> GetSingleUserProfileAsync(int userProfileId, int userId);
    Task<IEnumerable<UserProfileDTO>> GetManyUserProfilesAsync (int? userId);
}