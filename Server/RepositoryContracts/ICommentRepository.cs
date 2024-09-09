using Entities;

namespace RepositoryContracts;

public interface ICommentRepository {
    Task<Comment> AddAsync(Comment comment);
    Task UpdateAsync(Comment comment);
    Task DeleteAsync(int commentId, int postId, int forumId);
    Task<Comment> GetSingleAsync(int commentId, int postId, int forumId);
    IQueryable<Comment> GetMany();
}