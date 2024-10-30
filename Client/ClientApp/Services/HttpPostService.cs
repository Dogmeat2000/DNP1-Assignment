using System.Text.Json;
using ApiContracts;

namespace ClientApp.Services;

public class HttpPostService : IPostService {
    private readonly HttpClient client;
    
    public HttpPostService(HttpClient client) {
        this.client = client;
    }

    public async Task<PostDTO> AddPostAsync(PostDTO request) {
        throw new NotImplementedException();
        // TODO: Missing implementation
    }

    public async Task UpdatePostAsync(int postId, PostDTO request) {
        throw new NotImplementedException();
        // TODO: Missing implementation
    }

    public async Task DeletePostAsync(int postId, PostDTO request) {
        throw new NotImplementedException();
        // TODO: Missing implementation
    }

    public async Task<PostDTO> GetSinglePostAsync(int postId, int forumId) {
        HttpResponseMessage httpResponse = await client.GetAsync($"Posts/{postId}?fId={forumId}");
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode) {
            throw new KeyNotFoundException(response);
        }
        
        return JsonSerializer.Deserialize<PostDTO>(response, new JsonSerializerOptions {
            PropertyNameCaseInsensitive = true
        })!;
    }

    
    public async Task<IEnumerable<PostDTO>> GetManyPostsAsync(int forumId, String? searchTitle, int? authorId) {
        HttpResponseMessage httpResponse;
        
        if (searchTitle != null && authorId != null)
            httpResponse = await client.GetAsync($"Posts?fId={forumId}&searchString={searchTitle}&authorId={authorId}");
        else if (searchTitle != null)
            httpResponse = await client.GetAsync($"Posts?fId={forumId}&searchString={searchTitle}");
        else if (authorId != null)
            httpResponse = await client.GetAsync($"Posts?fId={forumId}&authorId={authorId}");
        else 
            httpResponse = await client.GetAsync($"Posts?fId={forumId}");
        
        string response = await httpResponse.Content.ReadAsStringAsync();
        
        if (!httpResponse.IsSuccessStatusCode) {
            throw new KeyNotFoundException(response);
        }
        
        return JsonSerializer.Deserialize<List<PostDTO>>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }) ?? Enumerable.Empty<PostDTO>();
    }
}