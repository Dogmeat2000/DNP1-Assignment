using System.Text.Json;
using ApiContracts;

namespace ClientApp.Services;

public class HttpCommentService : ICommentService {
    private readonly HttpClient client;
    
    public HttpCommentService(HttpClient client) {
        this.client = client;
    }

    public async Task<CommentDTO> AddCommentAsync(CommentDTO comment) {
        throw new NotImplementedException();
        // TODO: Missing implementation
    }

    public async Task UpdateCommentAsync(CommentDTO comment) {
        throw new NotImplementedException();
        // TODO: Missing implementation
    }

    public async Task DeleteCommentAsync(int commentId, int postId, int forumId) {
        throw new NotImplementedException();
        // TODO: Missing implementation
    }

    public async Task<CommentDTO> GetSingleCommentAsync(int commentId, int postId, int forumId) {
        throw new NotImplementedException();
        // TODO: Missing implementation
    }

    public async Task<IEnumerable<CommentDTO>> GetManyCommentsAsync(int forumId, int postId, int? authorId) {
        HttpResponseMessage httpResponse;
        
        if (authorId != null)
            httpResponse = await client.GetAsync($"Comments?fId={forumId}&pId={postId}&authorId={authorId}");
        else 
            httpResponse = await client.GetAsync($"Comments?fId={forumId}&pId={postId}");
        
        string response = await httpResponse.Content.ReadAsStringAsync();
        
        if (!httpResponse.IsSuccessStatusCode) {
            throw new KeyNotFoundException(response);
        }
        
        return JsonSerializer.Deserialize<List<CommentDTO>>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }) ?? Enumerable.Empty<CommentDTO>();
    }
}