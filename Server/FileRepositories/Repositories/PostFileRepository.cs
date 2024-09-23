using Entities;
using FileRepositories.Persistance;
using RepositoryContracts;

namespace FileRepositories.Repositories;

public class PostFileRepository : IPostRepository {
    private readonly string _filePath = Directory.GetCurrentDirectory() + @"\DataFiles\posts.json";
    private IFilePersistance FileManager { get; } = new FilePersistance();
    private List<Post> PostList { get; set; } = [];
    public string ErrorAddFailed { get; } = "Error occured while adding Post. Data failed to load.";
    public string ErrorUpdateFailed { get; } = "Error occured while updating Post. Data failed to load.";
    public string ErrorDeleteFailed { get; } = "Error occured while deleting Post. Data failed to load.";
    public string ErrorGetSingleFailed { get; } = "Error occured while retrieving a single Post. Data failed to load.";
    public string ErrorGetManyFailed { get; } = "Error occured while retrieving all Posts. Data failed to load.";

    
    public async Task<Post> AddAsync(Post post) {
                
        // Load raw data from file:
        List<object>? rawData = await FileManager.ReadFromJsonFileAsync(_filePath, new Post());
        
        // If data is not null, continue, else abort by throwing an exception!
        if (rawData != null) {
            
            // Cast the loaded data to the proper load format:
            PostList = rawData.OfType<Post>().ToList();
            
            // Identify a unique post_id to assign to this Post.
            post.Post_id = PostList.Any() 
                ? PostList.Max(p => p.Post_id) + 1 
                : 1;
            
            // Assign a time of creation:
            post.Timestamp_created = DateTime.Now;
            
            // Add the new Post to the list of Posts:
            PostList.Add(post);
            
            // Cast the modified data back to the proper save format, and attempt to save:
            if (await FileManager.SaveToJsonFileAsync(_filePath, PostList.Cast<object>().ToList())) {
                Console.WriteLine($": Added Post with ID '{post.Post_id}' in Forum '{post.ParentForum_id}'");
            } else {
                Console.WriteLine($": ERROR DID NOT SAVE Post with ID '{post.Post_id}' in Forum '{post.ParentForum_id}'");
            }

        } else {
            throw new Exception(ErrorAddFailed);
        }
        
        return post;
    }

    
    public async Task UpdateAsync(Post post) {
                
        // Load raw data from file:
        List<object>? rawData = await FileManager.ReadFromJsonFileAsync(_filePath, new Post());
        
        // If data is not null, continue, else abort by throwing an exception!
        if (rawData != null) {
            
            // Cast the loaded data to the proper load format:
            PostList = rawData.OfType<Post>().ToList();
            
            // Check if the Post to modify actually exists:
            Post? existingPost = PostList.SingleOrDefault(p => p.Post_id == post.Post_id && p.ParentForum_id == post.ParentForum_id);
            if (existingPost is null) {
                throw new InvalidOperationException($"Post with ID '{post.Post_id}' in Forum '{post.ParentForum_id}' not found");
            }
            
            // If it does exist, Remove it from the list of Comments:
            PostList.Remove(existingPost);
            
            // Assign/Update the modified date:
            post.Timestamp_modified = DateTime.Now;
            
            // Add the modified Post to the List
            PostList.Add(post);
            
            // Cast the modified data back to the proper save format, and attempt to save:
            if (await FileManager.SaveToJsonFileAsync(_filePath, PostList.Cast<object>().ToList())) {
                Console.WriteLine($": Modified Post with ID '{post.Post_id}' in Forum '{post.ParentForum_id}'");
                // TODO Update underlying Comments also! -> Should happen in business UI Logic, instead of here!
            } else {
                Console.WriteLine($": ERROR DID NOT Modify Post with ID '{post.Post_id}' in Forum '{post.ParentForum_id}'");
            }
        } else {
            throw new Exception(ErrorUpdateFailed);
        }
    }

    
    public async Task DeleteAsync(int postId, int parentForumId) {
                
        // Load raw data from file:
        List<object>? rawData = await FileManager.ReadFromJsonFileAsync(_filePath, new Post());
        
        // If data is not null, continue, else abort by throwing an exception!
        if (rawData != null) {
            
            // Cast the loaded data to the proper load format:
            PostList = rawData.OfType<Post>().ToList();
            
            // Check if the Post to remove actually exists:
            Post? postToRemove = PostList.SingleOrDefault(p => p.Post_id == postId && p.ParentForum_id == parentForumId);
            if (postToRemove is null) {
                throw new InvalidOperationException($"Post with ID '{postId}' in Forum '{parentForumId}' not found");
            }
            
            // If it does exist, Remove it from the list of Posts:
            PostList.Remove(postToRemove);
            
            // Cast the modified data back to the proper save format, and attempt to save:
            if (await FileManager.SaveToJsonFileAsync(_filePath, PostList.Cast<object>().ToList())) {
                Console.WriteLine($": DELETED Post with ID '{postToRemove.Post_id}' in Forum '{postToRemove.ParentForum_id}'");
                // TODO Delete underlying Comments also! -> Should happen in business UI Logic, instead of here!
            } else {
                Console.WriteLine($": ERROR DID NOT Delete Post with ID '{postToRemove.Post_id}' in Forum '{postToRemove.ParentForum_id}'");
            }
        } else {
            throw new Exception(ErrorDeleteFailed);
        }
    }

    
    public async Task<Post> GetSingleAsync(int postId, int parentForumId) {
                        
        // Load raw data from file:
        List<object>? rawData = await FileManager.ReadFromJsonFileAsync(_filePath, new Post());
        
        // If data is not null, continue, else abort by throwing an exception!
        if (rawData != null) {
            
            // Cast the loaded data to the proper load format:
            PostList = rawData.OfType<Post>().ToList();
            
            // Check if the Post actually exists:
            Post? postToReturn = PostList.SingleOrDefault(p => p.Post_id == postId && p.ParentForum_id == parentForumId);
            if (postToReturn is null) {
                throw new InvalidOperationException($"Post with ID '{postId}' Forum '{parentForumId}' not found");
            }
            
            // If it does exist, return it:
            return postToReturn;
        } 
        throw new Exception(ErrorGetSingleFailed);
    }

    
    public IQueryable<Post> GetMany() {
        // Load raw data from file:
        List<object>? rawData = FileManager.ReadFromJsonFileAsync(_filePath, new Post()).Result;
        
        // If data is not null, continue, else abort by throwing an exception!
        if (rawData != null) {
            
            // Cast the loaded data to the proper load format:
            PostList = rawData.OfType<Post>().ToList();
            
            // Return the entire list:
            return PostList.AsQueryable();
        } 
        throw new Exception(ErrorGetManyFailed);
    }
}