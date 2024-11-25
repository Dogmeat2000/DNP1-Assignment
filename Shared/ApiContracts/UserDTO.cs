namespace ApiContracts;

public class UserDTO {
    public int User_id { get; set; }
    public string? Username { get; set; } //Note; this is not part of the raw server User entity.
    public List<int>? ManagedForumIds { get; } = new();
    public List<int>? ManagedPostIds { get; } = new();
    public List<int>? ManagedCommentIds { get; } = new();
    
    public void addChildCommentId(int commentId) {
        ManagedCommentIds.Add(commentId);
    }

    public void deleteChildCommentId(int commentId) {
        ManagedCommentIds.Remove(commentId);
    }
    
    public void addChildPostId(int postId) {
        ManagedPostIds.Add(postId);
    }

    public void deleteChildPostId(int postId) {
        ManagedPostIds.Remove(postId);
    }
    
    public void addChildForumId(int forumId) {
        ManagedForumIds.Add(forumId);
    }

    public void deleteChildForumId(int forumId) {
        ManagedForumIds.Remove(forumId);
    }
}