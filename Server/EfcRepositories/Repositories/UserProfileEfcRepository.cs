using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RepositoryContracts;

namespace EfcRepositories.Repositories;

public class UserProfileEfcRepository : IUserProfileRepository {
    
    private readonly AppContext _context;

    public UserProfileEfcRepository(AppContext context) {
        _context = context;
    }
    
    public async Task<UserProfile> AddAsync(UserProfile userProfile) {
        EntityEntry<UserProfile> entityEntry = await _context.UserProfiles.AddAsync(userProfile);
        await _context.SaveChangesAsync();
        return entityEntry.Entity;
    }

    public async Task UpdateAsync(UserProfile userProfile) {
        if (!(await _context.UserProfiles.AnyAsync(uP => uP.Profile_id == userProfile.Profile_id))) {
            throw new KeyNotFoundException($"UserProfile with ID '{userProfile.Profile_id}' for User with ID {userProfile.User_id} not found");
        }
        _context.UserProfiles.Update(userProfile);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int profileId, int userId) {
        UserProfile? existing = await _context.UserProfiles.SingleOrDefaultAsync(uP => uP.Profile_id == profileId && uP.User_id == userId);
        if (existing == null) {
            throw new KeyNotFoundException($"UserProfile with ID '{profileId}' associated with User with ID '{userId}' not found");
        }
        _context.UserProfiles.Remove(existing);
        await _context.SaveChangesAsync();
    }

    public async Task<UserProfile> GetSingleAsync(int profileId, int userId) {
        UserProfile? existing = await _context.UserProfiles.SingleOrDefaultAsync(uP => uP.Profile_id == profileId && uP.User_id == userId);
        if (existing == null) {
            throw new KeyNotFoundException($"UserProfile with ID '{profileId}' associated with User with ID '{userId}' not found");
        }
        return existing;
    }

    public async Task<UserProfile> GetSingleAsync(string username) {
        UserProfile? existing = await _context.UserProfiles.SingleOrDefaultAsync(uP => uP.Username == username);
        if (existing == null) {
            throw new KeyNotFoundException($"No UserProfile with Username '{username}' could be found");
        }
        return existing;
    }

    public IQueryable<UserProfile> GetMany() {
        return _context.UserProfiles.AsQueryable();
    }
}