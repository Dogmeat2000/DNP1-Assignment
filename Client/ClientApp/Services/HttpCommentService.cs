namespace ClientApp.Services;

public class HttpCommentService : ICommentService {
    private readonly HttpClient client;
    
    public HttpCommentService(HttpClient client) {
        this.client = client;
    }
    
}