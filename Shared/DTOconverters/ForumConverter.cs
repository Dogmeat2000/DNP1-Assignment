using ApiContracts;
using Entities;

namespace DTOconverters;

public class ForumConverter {
    public static ForumDTO ForumToDTO(Forum forum) {
        return new ForumDTO() {
            Forum_id = forum.Forum_id,
            Title_txt = forum.Title_txt,
            Timestamp_created = forum.Timestamp_created,
            Timestamp_modified = forum.Timestamp_modified,
            Timestamp_deleted = forum.Timestamp_deleted,
            LastPost_id = forum.LastPost_id,
            LastCommentPost_id = forum.LastCommentPost_id,
            LastComment_id = forum.LastComment_id, 
            Author_id = forum.Author_id,
            ParentForum_id = forum.ParentForum_id,
        };
    }
    
    public static Forum DTOToForum(ForumDTO dtoObject) {
        return new Forum() {
            Forum_id = dtoObject.Forum_id,
            Title_txt = dtoObject.Title_txt,
            Timestamp_created = dtoObject.Timestamp_created,
            Timestamp_modified = dtoObject.Timestamp_modified,
            Timestamp_deleted = dtoObject.Timestamp_deleted,
            LastPost_id = dtoObject.LastPost_id,
            LastCommentPost_id = dtoObject.LastCommentPost_id,
            LastComment_id = dtoObject.LastComment_id, 
            Author_id = dtoObject.Author_id,
            ParentForum_id = dtoObject.ParentForum_id,
        };
    }
}