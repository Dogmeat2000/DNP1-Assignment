using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RepositoryContracts;

namespace EfcRepositories.Repositories;

public class UserEfcRepository : IUserRepository {
    
    private readonly LocalAppContext _context;

    public UserEfcRepository(LocalAppContext context) {
        _context = context;
    }
    
    public async Task<User> AddAsync(User user) {
        EntityEntry<User> entityEntry = await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return entityEntry.Entity;
    }

    public async Task UpdateAsync(User user) {
        if (!(await _context.Users.AnyAsync(u => u.User_id == user.User_id))) {
            throw new KeyNotFoundException($"Post with ID '{user.User_id}' not found");
        }
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int userId) {
        User? existing = await _context.Users.SingleOrDefaultAsync(u => u.User_id == userId);
        if (existing == null) {
            throw new KeyNotFoundException($"User with ID '{userId}' not found");
        }
        _context.Users.Remove(existing);
        await _context.SaveChangesAsync();
    }

    public async Task<User> GetSingleAsync(int userId) {
        User? existing = await _context.Users.SingleOrDefaultAsync(u => u.User_id == userId);
        if (existing == null) {
            throw new KeyNotFoundException($"User with ID '{userId}' not found");
        }
        return existing;
    }

    public IQueryable<User> GetMany() {
        return _context.Users.AsQueryable();
    }
}