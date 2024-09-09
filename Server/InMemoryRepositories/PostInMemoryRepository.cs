using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class PostInMemoryRepository : IPostRepository {
    
    public List<Post> PostList;

    public PostInMemoryRepository() {
        PostList = [];
        GenerateDummyData();
    }
    
    public Task<Post> AddAsync(Post post) {
        post.Post_id = PostList.Any() 
            ? PostList.Max(p => p.Post_id) + 1
            : 1;
        PostList.Add(post);
        return Task.FromResult(post);
    }

    public Task UpdateAsync(Post post) {
        Post ? existingPost = PostList.SingleOrDefault(p => p.Post_id == post.Post_id && p.ParentForum_id == post.ParentForum_id);
        if (existingPost is null) {
            throw new InvalidOperationException(
                $"Post with ID '{post.Post_id}' not found");
        }

        PostList.Remove(existingPost);
        PostList.Add(post);

        return Task.CompletedTask;
    }

    public Task DeleteAsync(int postId, int parentForumId) {
        Post ? postToRemove = PostList.SingleOrDefault(p => p.Post_id == postId && p.ParentForum_id == parentForumId);
        if (postToRemove is null) {
            throw new InvalidOperationException(
                $"Post with ID '{postId}' in forum '{parentForumId}' not found");
        }

        PostList.Remove(postToRemove);
        return Task.CompletedTask;
    }

    public Task<Post> GetSingleAsync(int postId, int parentForumId) {
        Post ? postToReturn = PostList.SingleOrDefault(p => p.Post_id == postId && p.ParentForum_id == parentForumId);
        if (postToReturn is null) {
            throw new InvalidOperationException(
                $"Post with ID '{postId}' in forum '{parentForumId}' not found");
        }
        
        return Task.FromResult(postToReturn);
    }

    public IQueryable<Post> GetMany() {
        return PostList.AsQueryable();
    }
    
    private void GenerateDummyData() {
        for (int i = 0; i <= 5; i++) {
            Post post = new Post();
            switch (i) {
                case 0:
                    post.Post_id = i;
                    post.Title_txt = "Test Post in SubForum 1-1";
                    post.Body_txt = "POST: Lorem Ipsum";
                    post.Timestamp_created = DateTime.Now;
                    post.Timestamp_modified = DateTime.MinValue;
                    post.Timestamp_deleted = DateTime.MinValue;
                    post.ParentForum_id = 2;
                    post.Author_id = i;
                    break;
                
                case 1: 
                    post.Post_id = i;
                    post.Title_txt = "Test Post in Main Forum 1";
                    post.Body_txt = "POST: Lorem Ipsum";
                    post.Timestamp_created = DateTime.Now;
                    post.Timestamp_modified = DateTime.MinValue;
                    post.Timestamp_deleted = DateTime.MinValue;
                    post.ParentForum_id = 0;
                    post.Author_id = i;
                    break;
                
                case 2: 
                    post.Post_id = i;
                    post.Title_txt = "Test Post in Main Forum 2";
                    post.Body_txt = "POST: Lorem Ipsum";
                    post.Timestamp_created = DateTime.Now;
                    post.Timestamp_modified = DateTime.MinValue;
                    post.Timestamp_deleted = DateTime.MinValue;
                    post.ParentForum_id = 1;
                    post.Author_id = i;
                    break;
                
                case 3: 
                    post.Post_id = i;
                    post.Title_txt = "Test Post in SubForum 1-2";
                    post.Body_txt = "POST: Lorem Ipsum";
                    post.Timestamp_created = DateTime.Now;
                    post.Timestamp_modified = DateTime.MinValue;
                    post.Timestamp_deleted = DateTime.MinValue;
                    post.ParentForum_id = 3;
                    post.Author_id = i;
                    break;
                
                case 4: 
                    post.Post_id = i;
                    post.Title_txt = "Test Post in SubForum 2-2";
                    post.Body_txt = "POST: Lorem Ipsum";
                    post.Timestamp_created = DateTime.Now;
                    post.Timestamp_modified = DateTime.MinValue;
                    post.Timestamp_deleted = DateTime.MinValue;
                    post.ParentForum_id = 5;
                    post.Author_id = i;
                    break;
                
                case 5: 
                    post.Post_id = i;
                    post.Title_txt = "Test Post in SubForum 2-1";
                    post.Body_txt = "POST: Lorem Ipsum";
                    post.Timestamp_created = DateTime.Now;
                    post.Timestamp_modified = DateTime.MinValue;
                    post.Timestamp_deleted = DateTime.MinValue;
                    post.ParentForum_id = 4;
                    post.Author_id = i;
                    break;
                
                default: 
                    break;
            }
            PostList.Add(post);
        }
    }
}