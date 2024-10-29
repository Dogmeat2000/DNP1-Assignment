using ApiContracts;

namespace ClientApp.Services;

public interface IForumService {
    Task<ForumDTO> AddAsync(ForumDTO forum);
    Task UpdateAsync(ForumDTO forum);
    Task DeleteAsync(int forumId, int parentForumId);
    Task<ForumDTO> GetSingleAsync(int forumId, int parentForumId);
    Task<IEnumerable<ForumDTO>> GetManyForumsAsync(int parentForumId);
}