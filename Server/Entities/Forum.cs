namespace Entities;

public class Forum
{
    public int Forum_id { get; set; }
    public string Title_txt { get; set; }
    public DateTime Timestamp_created { get; set; }
    public DateTime Timestamp_modified { get; set; }
    public DateTime Timestamp_deleted { get; set; }
    public int LastPost_id { get; set; }
    public int LastCommentPost_id { get; set; }
    public int LastComment_id { get; set; }
    public int Author_id { get; set; }
    public int ParentForum_id { get; set; }

    public Forum() {
        Title_txt = "";
        //TODO
    }
}