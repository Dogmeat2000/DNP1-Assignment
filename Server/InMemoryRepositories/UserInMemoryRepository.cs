using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class UserInMemoryRepository : IUserRepository {
    
    public List<User> UserList;

    public UserInMemoryRepository() {
        UserList = [];
        GenerateDummyData();
    }
    
    public Task<User> AddAsync(User user) {
        user.User_id = UserList.Any() 
            ? UserList.Max(u => u.User_id) + 1
            : 1;
        UserList.Add(user);
        return Task.FromResult(user);
    }

    public Task UpdateAsync(User user) {
        User? existingUser = UserList.SingleOrDefault(u => u.User_id == user.User_id);
        if (existingUser is null) {
            throw new InvalidOperationException(
                $"User with ID '{user.User_id}' not found");
        }

        UserList.Remove(existingUser);
        UserList.Add(user);

        return Task.CompletedTask;
    }

    public Task DeleteAsync(int userId) {
        User? userToRemove = UserList.SingleOrDefault(u => u.User_id == userId);
        if (userToRemove is null) {
            throw new InvalidOperationException(
                $"User with ID '{userId}' not found");
        }

        UserList.Remove(userToRemove);
        return Task.CompletedTask;
    }

    public Task<User> GetSingleAsync(int userId) {
        User? userToReturn = UserList.SingleOrDefault(u => u.User_id == userId);
        if (userToReturn is null) {
            throw new InvalidOperationException(
                $"User with ID '{userId}' not found");
        }
        
        return Task.FromResult(userToReturn);
    }

    public IQueryable<User> GetMany() {
        return UserList.AsQueryable();
    }

    private void GenerateDummyData() {
        for (int i = 0; i <= 5; i++) {
            User user = new User(i);
            UserList.Add(user);
        }
    }
}