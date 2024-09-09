using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class ForumInMemoryRepository : IForumRepository {
    
    public List<Forum> ForumList;

    public ForumInMemoryRepository() {
        ForumList = [];
    }
    
    public Task<Forum> AddAsync(Forum forum) {
        forum.Forum_id = ForumList.Any() 
            ? ForumList.Max(f => f.Forum_id) + 1
            : 1;
        ForumList.Add(forum);
        return Task.FromResult(forum);
    }

    public Task UpdateAsync(Forum forum) {
        Forum ? existingForum = ForumList.SingleOrDefault(f => f.Forum_id == forum.Forum_id && f.ParentForum_id == forum.ParentForum_id);
        if (existingForum is null) {
            throw new InvalidOperationException(
                $"Forum with ID '{forum.Forum_id}' in Forum '{forum.ParentForum_id}' not found");
        }

        ForumList.Remove(existingForum);
        ForumList.Add(forum);

        return Task.CompletedTask;
    }

    public Task DeleteAsync(int forumId, int parentForumId) {
        Forum ? forumToRemove = ForumList.SingleOrDefault(f => f.Forum_id == forumId && f.ParentForum_id == parentForumId);
        if (forumToRemove is null) {
            throw new InvalidOperationException(
                $"Forum with ID '{forumId}' in Forum '{parentForumId}' not found");
        }

        ForumList.Remove(forumToRemove);
        return Task.CompletedTask;
    }

    public Task<Forum> GetSingleAsync(int forumId, int parentForumId) {
        Forum ? forumToReturn = ForumList.SingleOrDefault(f => f.Forum_id == forumId && f.ParentForum_id == parentForumId);
        if (forumToReturn is null) {
            throw new InvalidOperationException(
                $"Forum with ID '{forumId}' in Forum '{parentForumId}' not found");
        }
        
        return Task.FromResult(forumToReturn);
    }

    public IQueryable<Forum> GetMany() {
        return ForumList.AsQueryable();
    }
}