using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class CommentInMemoryRepository : ICommentRepository {
    
    public List<Comment> CommentList;

    public CommentInMemoryRepository() {
        CommentList = [];
        GenerateDummyData();
    }
    
    
    public Task<Comment> AddAsync(Comment comment) {
        comment.Comment_id = CommentList.Any() 
            ? CommentList.Max(c => c.Comment_id) + 1
            : 1;
        CommentList.Add(comment);
        return Task.FromResult(comment);
    }

    public Task UpdateAsync(Comment comment) {
        Comment? existingComment = CommentList.SingleOrDefault(c => c.Comment_id == comment.Comment_id && c.ParentPost_id == comment.ParentPost_id && c.ParentForum_id == comment.ParentForum_id);
        if (existingComment is null) {
            throw new InvalidOperationException(
                $"Comment with ID '{comment.Comment_id}' in Post '{comment.ParentPost_id}' in Forum '{comment.ParentForum_id}' not found");
        }

        CommentList.Remove(existingComment);
        CommentList.Add(comment);

        return Task.CompletedTask;
    }

    public Task DeleteAsync(int commentId, int postId, int forumId) {
        Comment? commentToRemove = CommentList.SingleOrDefault(c => c.Comment_id == commentId && c.ParentPost_id == postId && c.ParentForum_id == forumId);
        if (commentToRemove is null) {
            throw new InvalidOperationException(
                $"Comment with ID '{commentId}' in Post '{postId}' in Forum '{forumId}' not found");
        }

        CommentList.Remove(commentToRemove);
        return Task.CompletedTask;
    }

    public Task<Comment> GetSingleAsync(int commentId, int postId, int forumId) {
        Comment? commentToReturn = CommentList.SingleOrDefault(c => c.Comment_id == commentId && c.ParentPost_id == postId && c.ParentForum_id == forumId);
        if (commentToReturn is null) {
            throw new InvalidOperationException(
                $"Comment with ID '{commentId}' in Post '{postId}' in Forum '{forumId}' not found");
        }
        
        return Task.FromResult(commentToReturn);
    }

    public IQueryable<Comment> GetMany() {
        return CommentList.AsQueryable();
    }
    
    private void GenerateDummyData() {
        for (int i = 0; i <= 5; i++) {
            Comment comment = new Comment();
            switch (i) {
                case 0:
                    comment.Comment_id = i;
                    comment.Body_txt = "COMMENT: Lorem Ipsum";
                    comment.Timestamp_created = DateTime.Now;
                    comment.Timestamp_modified = DateTime.MinValue;
                    comment.Timestamp_deleted = DateTime.MinValue;
                    comment.ParentPost_id = i;
                    comment.ParentForum_id = 2;
                    comment.Author_Id = 5 - i;
                    break;
                
                case 1: 
                    comment.Comment_id = i;
                    comment.Body_txt = "COMMENT: Lorem Ipsum";
                    comment.Timestamp_created = DateTime.Now;
                    comment.Timestamp_modified = DateTime.MinValue;
                    comment.Timestamp_deleted = DateTime.MinValue;
                    comment.ParentPost_id = i;
                    comment.ParentForum_id = 0;
                    comment.Author_Id = 5 - i;
                    break;
                
                case 2: 
                    comment.Comment_id = i;
                    comment.Body_txt = "COMMENT: Lorem Ipsum";
                    comment.Timestamp_created = DateTime.Now;
                    comment.Timestamp_modified = DateTime.MinValue;
                    comment.Timestamp_deleted = DateTime.MinValue;
                    comment.ParentPost_id = i;
                    comment.ParentForum_id = 1;
                    comment.Author_Id = 5 - i;
                    break;
                
                case 3: 
                    comment.Comment_id = i;
                    comment.Body_txt = "COMMENT: Lorem Ipsum";
                    comment.Timestamp_created = DateTime.Now;
                    comment.Timestamp_modified = DateTime.MinValue;
                    comment.Timestamp_deleted = DateTime.MinValue;
                    comment.ParentPost_id = i;
                    comment.ParentForum_id = 3;
                    comment.Author_Id = 5 - i;
                    break;
                
                case 4: 
                    comment.Comment_id = i;
                    comment.Body_txt = "COMMENT: Lorem Ipsum";
                    comment.Timestamp_created = DateTime.Now;
                    comment.Timestamp_modified = DateTime.MinValue;
                    comment.Timestamp_deleted = DateTime.MinValue;
                    comment.ParentPost_id = i;
                    comment.ParentForum_id = 5;
                    comment.Author_Id = 5 - i;
                    break;
                
                case 5: 
                    comment.Comment_id = i;
                    comment.Body_txt = "COMMENT: Lorem Ipsum";
                    comment.Timestamp_created = DateTime.Now;
                    comment.Timestamp_modified = DateTime.MinValue;
                    comment.Timestamp_deleted = DateTime.MinValue;
                    comment.ParentPost_id = i;
                    comment.ParentForum_id = 4;
                    comment.Author_Id = 5 - i;
                    break;
                
                default: 
                    break;
            }
            CommentList.Add(comment);
        }
    }
}