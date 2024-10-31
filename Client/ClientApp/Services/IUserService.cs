using ApiContracts;

namespace ClientApp.Services;

public interface IUserService {
    Task<UserDTO> AddUserAsync(UserDTO request);
    Task UpdateUserAsync(int id, UserDTO request);
    Task DeleteUserAsync(int userId, UserDTO request);
    Task<UserDTO> GetSingleUserAsync(int userId);
    Task<IEnumerable<UserDTO>> GetManyUsersAsync ();
}