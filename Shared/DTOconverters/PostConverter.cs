using ApiContracts;
using Entities;

namespace DTOconverters;

public class PostConverter {
    public static PostDTO PostToDTO(Post post) {
        return new PostDTO() {
            Post_id = post.Post_id,
            Title_txt = post.Title_txt,
            Body_txt = post.Body_txt,
            Timestamp_created = post.Timestamp_created,
            Timestamp_modified = post.Timestamp_modified,
            Timestamp_deleted = post.Timestamp_deleted,
            Author_id = post.Author_id,
            ParentForum_id = post.ParentForum_id,
        };
    }
    
    public static Post DTOToPost(PostDTO dtoObject) {
        return new Post() {
            Post_id = dtoObject.Post_id,
            Title_txt = dtoObject.Title_txt,
            Body_txt = dtoObject.Body_txt,
            Timestamp_created = dtoObject.Timestamp_created,
            Timestamp_modified = dtoObject.Timestamp_modified,
            Timestamp_deleted = dtoObject.Timestamp_deleted,
            Author_id = dtoObject.Author_id,
            ParentForum_id = dtoObject.ParentForum_id,
        };
    }
}