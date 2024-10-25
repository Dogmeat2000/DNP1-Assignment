namespace ClientApp.Entities;

public class Comment {
    public int Comment_id { get; set; }
    public string Body_txt { get; set; }
    public DateTime Timestamp_created { get; set; }
    public DateTime Timestamp_modified { get; set; }
    public DateTime Timestamp_deleted { get; set; }
    public int ParentPost_id { get; set; }
    public int ParentForum_id { get; set; }
    public int Author_Id { get; set; }

    public Comment() {
        Body_txt = "";
    }
}