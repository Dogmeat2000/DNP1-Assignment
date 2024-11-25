using Entities;

namespace RepositoryContracts;

public interface IForumRepository {
    Task<Forum> AddAsync(Forum forum);
    Task UpdateAsync(Forum forum);
    Task DeleteAsync(int forumId, int? parentForumId);
    Task<Forum> GetSingleAsync(int forumId, int? parentForumId);
    IQueryable<Forum> GetMany();
}