using Entities;

namespace RepositoryContracts;

public interface IPostRepository {
    Task<Post> AddAsync(Post post);
    Task UpdateAsync(Post post);
    Task DeleteAsync(int postId, int? parentForumId);
    Task<Post> GetSingleAsync(int postId, int? parentForumId);
    IQueryable<Post> GetMany();
}