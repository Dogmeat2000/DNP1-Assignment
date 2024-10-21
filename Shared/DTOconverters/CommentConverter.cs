using ApiContracts;
using Entities;

namespace DTOconverters;

public class CommentConverter {
    public static CommentDTO CommentToDTO(Comment comment) {
        return new CommentDTO() {
            Comment_id = comment.Comment_id,
            Author_Id = comment.Author_Id,
            Body_txt = comment.Body_txt,
            Timestamp_created = comment.Timestamp_created,
            Timestamp_modified = comment.Timestamp_modified,
            Timestamp_deleted = comment.Timestamp_deleted,
            ParentPost_id = comment.ParentPost_id,
            ParentForum_id = comment.ParentForum_id,
        };
    }
    
    public static Comment DTOToComment(CommentDTO dtoObject) {
        return new Comment() {
            Comment_id = dtoObject.Comment_id,
            Author_Id = dtoObject.Author_Id,
            Body_txt = dtoObject.Body_txt,
            Timestamp_created = dtoObject.Timestamp_created,
            Timestamp_modified = dtoObject.Timestamp_modified,
            Timestamp_deleted = dtoObject.Timestamp_deleted,
            ParentPost_id = dtoObject.ParentPost_id,
            ParentForum_id = dtoObject.ParentForum_id,
        };
    }
}