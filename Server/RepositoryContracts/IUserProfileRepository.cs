using Entities;

namespace RepositoryContracts;

public interface IUserProfileRepository {
    Task<UserProfile> AddAsync(UserProfile userProfile);
    Task UpdateAsync(UserProfile userProfile);
    Task DeleteAsync(int profileId, int userId);
    Task<UserProfile> GetSingleAsync(int profileId, int userId);
    Task<UserProfile> GetSingleAsync(string username);
    IQueryable<UserProfile> GetMany();
}