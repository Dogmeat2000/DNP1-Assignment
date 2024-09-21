using Entities;
using RepositoryContracts;

namespace FileRepositories.Repositories;

public class CommentFileRepository : ICommentRepository {

    private readonly string filePath = "comments.json";
    private List<Comment> _commentList;

    public CommentFileRepository() {
        _commentList = [];
    }
    
    
    public Task<Comment> AddAsync(Comment comment) {
        comment.Comment_id = _commentList.Any() 
            ? _commentList.Max(c => c.Comment_id) + 1
            : 1;
        _commentList.Add(comment);
        return Task.FromResult(comment);
    }

    public Task UpdateAsync(Comment comment) {
        Comment? existingComment = _commentList.SingleOrDefault(c => c.Comment_id == comment.Comment_id && c.ParentPost_id == comment.ParentPost_id && c.ParentForum_id == comment.ParentForum_id);
        if (existingComment is null) {
            throw new InvalidOperationException(
                $"Comment with ID '{comment.Comment_id}' in Post '{comment.ParentPost_id}' in Forum '{comment.ParentForum_id}' not found");
        }

        _commentList.Remove(existingComment);
        _commentList.Add(comment);

        return Task.CompletedTask;
    }

    public Task DeleteAsync(int commentId, int postId, int forumId) {
        Comment? commentToRemove = _commentList.SingleOrDefault(c => c.Comment_id == commentId && c.ParentPost_id == postId && c.ParentForum_id == forumId);
        if (commentToRemove is null) {
            throw new InvalidOperationException(
                $"Comment with ID '{commentId}' in Post '{postId}' in Forum '{forumId}' not found");
        }

        _commentList.Remove(commentToRemove);
        return Task.CompletedTask;
    }

    public Task<Comment> GetSingleAsync(int commentId, int postId, int forumId) {
        Comment? commentToReturn = _commentList.SingleOrDefault(c => c.Comment_id == commentId && c.ParentPost_id == postId && c.ParentForum_id == forumId);
        if (commentToReturn is null) {
            throw new InvalidOperationException(
                $"Comment with ID '{commentId}' in Post '{postId}' in Forum '{forumId}' not found");
        }
        
        return Task.FromResult(commentToReturn);
    }

    public IQueryable<Comment> GetMany() {
        return _commentList.AsQueryable();
    }
}