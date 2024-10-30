using System.Text.Json;
using ApiContracts;

namespace ClientApp.Services;

public class HttpForumService : IForumService {
    
    private readonly HttpClient client;
    
    public HttpForumService(HttpClient client) {
        this.client = client;
    }
    
    public async Task<ForumDTO> AddAsync(ForumDTO forum) {
        throw new NotImplementedException();
        // TODO: Missing Implementation
    }

    public async Task UpdateAsync(ForumDTO forum) {
        throw new NotImplementedException();
        // TODO: Missing Implementation
    }

    public async Task DeleteAsync(int forumId, int parentForumId) {
        throw new NotImplementedException();
        // TODO: Missing Implementation
    }

    public async Task<ForumDTO> GetSingleAsync(int forumId, int parentForumId) {
        HttpResponseMessage httpResponse = await client.GetAsync($"Forums/{forumId}?parentForumId={parentForumId}");
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode) {
            throw new KeyNotFoundException(response);
        }
        
        return JsonSerializer.Deserialize<ForumDTO>(response, new JsonSerializerOptions {
            PropertyNameCaseInsensitive = true
        })!;
    }

    public async Task<IEnumerable<ForumDTO>> GetManyForumsAsync(int parentForumId) {
        HttpResponseMessage httpResponse = await client.GetAsync($"Forums?parentForumId={parentForumId}");
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode) {
            throw new KeyNotFoundException(response);
        }
        
        return JsonSerializer.Deserialize<List<ForumDTO>>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }) ?? Enumerable.Empty<ForumDTO>();
    }
}