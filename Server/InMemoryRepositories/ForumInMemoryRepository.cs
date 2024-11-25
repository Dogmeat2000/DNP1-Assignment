using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class ForumInMemoryRepository : IForumRepository {
    
    public List<Forum> ForumList;

    public ForumInMemoryRepository() {
        ForumList = [];
        GenerateDummyData();
    }
    
    public Task<Forum> AddAsync(Forum forum) {
        forum.Forum_id = ForumList.Any() 
            ? ForumList.Max(f => f.Forum_id) + 1
            : 1;
        ForumList.Add(forum);
        return Task.FromResult(forum);
    }

    public Task UpdateAsync(Forum forum) {
        Forum? existingForum = ForumList.SingleOrDefault(f => f.Forum_id == forum.Forum_id && f.ParentForum_id == forum.ParentForum_id);
        if (existingForum is null) {
            throw new InvalidOperationException(
                $"Forum with ID '{forum.Forum_id}' in Forum '{forum.ParentForum_id}' not found");
        }

        ForumList.Remove(existingForum);
        ForumList.Add(forum);

        return Task.CompletedTask;
    }

    public Task DeleteAsync(int forumId, int parentForumId) {
        Forum? forumToRemove = ForumList.SingleOrDefault(f => f.Forum_id == forumId && f.ParentForum_id == parentForumId);
        if (forumToRemove is null) {
            throw new InvalidOperationException(
                $"Forum with ID '{forumId}' in Forum '{parentForumId}' not found");
        }

        ForumList.Remove(forumToRemove);
        return Task.CompletedTask;
    }

    public Task<Forum> GetSingleAsync(int forumId, int parentForumId) {
        Forum? forumToReturn = ForumList.SingleOrDefault(f => f.Forum_id == forumId && f.ParentForum_id == parentForumId);
        if (forumToReturn is null) {
            throw new InvalidOperationException(
                $"Forum with ID '{forumId}' in Forum '{parentForumId}' not found");
        }
        
        return Task.FromResult(forumToReturn);
    }

    public IQueryable<Forum> GetMany() {
        return ForumList.AsQueryable();
    }
    
    private void GenerateDummyData() {
        for (int i = 0; i <= 5; i++) {
            Forum forum = new Forum {
                Title_txt = ""
            };
            
            switch (i) {
                case 0:
                    forum.Forum_id = i;
                    forum.Title_txt = "Forum 1";
                    forum.Timestamp_created = DateTime.Now;
                    forum.Timestamp_modified = DateTime.MinValue;
                    forum.Timestamp_deleted = DateTime.MinValue;
                    forum.LastPost_id = 1;
                    forum.LastCommentPost_id = 1;
                    forum.LastComment_id = 1;
                    forum.Author_id = 0;
                    forum.ParentForum_id = -1;
                    break;
                
                case 1: 
                    forum.Forum_id = i;
                    forum.Title_txt = "Forum 2";
                    forum.Timestamp_created = DateTime.Now;
                    forum.Timestamp_modified = DateTime.MinValue;
                    forum.Timestamp_deleted = DateTime.MinValue;
                    forum.LastPost_id = 2;
                    forum.LastCommentPost_id = 2;
                    forum.LastComment_id = 2;
                    forum.Author_id = 1;
                    forum.ParentForum_id = -1;
                    break;
                
                case 2: 
                    forum.Forum_id = i;
                    forum.Title_txt = "SubForum 1-1";
                    forum.Timestamp_created = DateTime.Now;
                    forum.Timestamp_modified = DateTime.MinValue;
                    forum.Timestamp_deleted = DateTime.MinValue;
                    forum.LastPost_id = 0;
                    forum.LastCommentPost_id = 0;
                    forum.LastComment_id = 0;
                    forum.Author_id = 0;
                    forum.ParentForum_id = 0;
                    break;
                
                case 3: 
                    forum.Forum_id = i;
                    forum.Title_txt = "SubForum 1-2";
                    forum.Timestamp_created = DateTime.Now;
                    forum.Timestamp_modified = DateTime.MinValue;
                    forum.Timestamp_deleted = DateTime.MinValue;
                    forum.LastPost_id = 3;
                    forum.LastCommentPost_id = 3;
                    forum.LastComment_id = 3;
                    forum.Author_id = 0;
                    forum.ParentForum_id = 0;
                    break;
                
                case 4: 
                    forum.Forum_id = i;
                    forum.Title_txt = "SubForum 2-1";
                    forum.Timestamp_created = DateTime.Now;
                    forum.Timestamp_modified = DateTime.MinValue;
                    forum.Timestamp_deleted = DateTime.MinValue;
                    forum.LastPost_id = 5;
                    forum.LastCommentPost_id = 5;
                    forum.LastComment_id = 5;
                    forum.Author_id = 4;
                    forum.ParentForum_id = 2;
                    break;
                
                case 5: 
                    forum.Forum_id = i;
                    forum.Title_txt = "SubForum 2-2";
                    forum.Timestamp_created = DateTime.Now;
                    forum.Timestamp_modified = DateTime.MinValue;
                    forum.Timestamp_deleted = DateTime.MinValue;
                    forum.LastPost_id = 4;
                    forum.LastCommentPost_id = 4;
                    forum.LastComment_id = 4;
                    forum.Author_id = 1;
                    forum.ParentForum_id = 2;
                    break;
                
                default: 
                    break;
            }
            ForumList.Add(forum);
        }
    }
}