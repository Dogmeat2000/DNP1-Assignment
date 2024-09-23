namespace Entities;

public class Post
{
    public int Post_id { get; set; }
    public string Title_txt { get; set; }
    public string Body_txt { get; set; }
    public DateTime Timestamp_created  { get; set; }
    public DateTime Timestamp_modified { get; set; }
    public DateTime Timestamp_deleted { get; set; }
    public int ParentForum_id  { get; set; }
    public int Author_id  { get; set; }

    public Post() {
        Title_txt = "";
        Body_txt = "";
    }
}