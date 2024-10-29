using ApiContracts;

namespace ClientApp.Services;

public interface IPostService {
    Task<PostDTO> AddPostAsync(PostDTO request);
    Task UpdatePostAsync(int postId, PostDTO request);
    Task DeletePostAsync(int postId, PostDTO request);
    Task<PostDTO> GetSinglePostAsync(int postId, int forumId);
    Task<IEnumerable<PostDTO>> GetManyPostsAsync (int forumId, String? searchTitle, int? authorId);
}