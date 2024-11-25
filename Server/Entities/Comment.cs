namespace Entities;

public class Comment
{
    public int Comment_id { get; set; }
    public required string Body_txt { get; set; }
    public DateTime Timestamp_created { get; set; }
    public DateTime? Timestamp_modified { get; set; }
    public DateTime? Timestamp_deleted { get; set; }
    public int ParentPost_id { get; set; }
    public int? ParentForum_id { get; set; }
    public int? Author_Id { get; set; }

    // Navigation Properties:
    public User? AuthoringUser { get; set; } = null!;
    public Post ParentPost { get; set; } = null!;
    
    // Required by EFC:
    public Comment() {}
}