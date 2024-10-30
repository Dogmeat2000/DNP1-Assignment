using ApiContracts;

namespace ClientApp.Services;

public interface ICommentService {
    Task<CommentDTO> AddCommentAsync(CommentDTO comment);
    Task UpdateCommentAsync(CommentDTO comment);
    Task DeleteCommentAsync(int commentId, int postId, int forumId);
    Task<CommentDTO> GetSingleCommentAsync(int commentId, int postId, int forumId);
    Task<IEnumerable<CommentDTO>> GetManyCommentsAsync(int forumId, int postId, int? authorId);
}