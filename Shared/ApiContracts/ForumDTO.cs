namespace ApiContracts;

public class ForumDTO {
    public int Forum_id { get; set; }
    public string Title_txt { get; set; }
    public DateTime Timestamp_created { get; set; }
    public DateTime? Timestamp_modified { get; set; }
    public DateTime? Timestamp_deleted { get; set; }
    public int? LastPost_id { get; set; }
    public int? LastCommentPost_id { get; set; }
    public int? LastComment_id { get; set; }
    public int Author_id { get; set; }
    public int? ParentForum_id { get; set; }
    public List<int>? ChildPostIds { get; } = new();
    public List<int>? ChildForumIds { get; } = new();

    public void addChildPostId(int post_id) {
        ChildPostIds.Add(post_id);
    }

    public void deleteChildPostId(int post_id) {
        ChildPostIds.Remove(post_id);
    }
    
    public void addChildForumId(int forum_id) {
        ChildForumIds.Add(forum_id);
    }

    public void deleteChildForumId(int forum_id) {
        ChildForumIds.Remove(forum_id);
    }
    
}