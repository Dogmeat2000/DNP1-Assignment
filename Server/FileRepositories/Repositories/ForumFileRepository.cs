using Entities;
using FileRepositories.Persistance;
using RepositoryContracts;

namespace FileRepositories.Repositories;

public class ForumFileRepository : IForumRepository {
    private readonly string _filePath = Directory.GetCurrentDirectory() + @"\DataFiles\forums.json";
    private IFilePersistance FileManager { get; } = new FilePersistance();
    private List<Forum> ForumList { get; set; } = [];
    public string ErrorAddFailed { get; } = "Error occured while adding Forum. Data failed to load.";
    public string ErrorUpdateFailed { get; } = "Error occured while updating Forum. Data failed to load.";
    public string ErrorDeleteFailed { get; } = "Error occured while deleting Forum. Data failed to load.";
    public string ErrorGetSingleFailed { get; } = "Error occured while retrieving a single Forum. Data failed to load.";
    public string ErrorGetManyFailed { get; } = "Error occured while retrieving all Forums. Data failed to load.";

    
    public async Task<Forum> AddAsync(Forum forum) {
                
        // Load raw data from file:
        List<object>? rawData = await FileManager.ReadFromJsonFileAsync(_filePath, new Forum());
        
        // If data is not null, continue, else abort by throwing an exception!
        if (rawData != null) {
            
            // Cast the loaded data to the proper load format:
            ForumList = rawData.OfType<Forum>().ToList();
            
            // Select a unique id for this Forum instance:
            forum.Forum_id = ForumList.Any() 
                ? ForumList.Max(f => f.Forum_id) + 1 
                : 1;
            
            // Assign a time of creation:
            forum.Timestamp_created = DateTime.Now;
            
            // Add the Forum to the List:
            ForumList.Add(forum);
            
            // Cast the modified data back to the proper save format, and attempt to save:
            if (await FileManager.SaveToJsonFileAsync(_filePath, ForumList.Cast<object>().ToList())) {
                Console.WriteLine($": Added Forum with ID '{forum.Forum_id}'");
            } else {
                Console.WriteLine($": ERROR DID NOT SAVE Forum with ID '{forum.Forum_id}'");
            }

        } else {
            throw new Exception(ErrorAddFailed);
        }
        
        return forum;
    }

    
    public async Task UpdateAsync(Forum forum) {
                
        // Load raw data from file:
        List<object>? rawData = await FileManager.ReadFromJsonFileAsync(_filePath, new Forum());
        
        // If data is not null, continue, else abort by throwing an exception!
        if (rawData != null) {
            
            // Cast the loaded data to the proper load format:
            ForumList = rawData.OfType<Forum>().ToList();
            
            // Check if the Forum to modify actually exists:
            Forum? existingForum = ForumList.SingleOrDefault(f => f.Forum_id == forum.Forum_id && f.ParentForum_id == forum.ParentForum_id);
            if (existingForum is null) {
                throw new InvalidOperationException($"Forum with ID '{forum.Forum_id}' not found");
            }
            
            // If it does exist, Remove it from the list of Forums:
            ForumList.Remove(existingForum);
            
            // Assign/Update the modified date:
            forum.Timestamp_modified = DateTime.Now;
            
            // Add the modified Forum to the list:
            ForumList.Add(forum);
            
            // Cast the modified data back to the proper save format, and attempt to save:
            if (await FileManager.SaveToJsonFileAsync(_filePath, ForumList.Cast<object>().ToList())) {
                Console.WriteLine($": Modified Forum with ID '{forum.Forum_id}'");
                
                // TODO Update underlying Posts and Comments also! -> Should happen in business UI Logic, instead of here!
            } else {
                Console.WriteLine($": ERROR DID NOT Modify Forum with ID '{forum.Forum_id}'");
            }

        } else {
            throw new Exception(ErrorUpdateFailed);
        }
    }

    
    public async Task DeleteAsync(int forumId, int parentForumId) {
                
        // Load raw data from file:
        List<object>? rawData = await FileManager.ReadFromJsonFileAsync(_filePath, new Forum());
        
        // If data is not null, continue, else abort by throwing an exception!
        if (rawData != null) {
            
            // Cast the loaded data to the proper load format:
            ForumList = rawData.OfType<Forum>().ToList();
            
            // Check if the Forum to remove actually exists:
            Forum? forumToRemove = ForumList.SingleOrDefault(f => f.Forum_id == forumId && f.ParentForum_id == parentForumId);
            if (forumToRemove is null) {
                throw new InvalidOperationException($"Forum with ID '{forumId}' not found");
            }
            
            // If it does exist, Remove it from the list of Forums:
            ForumList.Remove(forumToRemove);
            
            // Cast the modified data back to the proper save format, and attempt to save:
            if (await FileManager.SaveToJsonFileAsync(_filePath, ForumList.Cast<object>().ToList())) {
                Console.WriteLine($": DELETED Forum with ID '{forumToRemove.Forum_id}'");
                // TODO: Delete all underlying Posts and Comments also!
            } else {
                Console.WriteLine($": ERROR DID NOT Delete Forum with ID '{forumToRemove.Forum_id}'");
            }
        } else {
            throw new Exception(ErrorDeleteFailed);
        }
    }

    
    public async Task<Forum> GetSingleAsync(int forumId, int parentForumId) {
                        
        // Load raw data from file:
        List<object>? rawData = await FileManager.ReadFromJsonFileAsync(_filePath, new Forum());
        
        // If data is not null, continue, else abort by throwing an exception!
        if (rawData != null) {
            
            // Cast the loaded data to the proper load format:
            ForumList = rawData.OfType<Forum>().ToList();
            
            // Check if the Forum actually exists:
            Forum? forumToReturn = ForumList.SingleOrDefault(f => f.Forum_id == forumId && f.ParentForum_id == parentForumId);
            if (forumToReturn is null) {
                throw new InvalidOperationException($"Forum with ID '{forumId}' not found");
            }
            
            // If it does exist, return it:
            return forumToReturn;
        } 
        throw new Exception(ErrorGetSingleFailed);
    }

    
    public IQueryable<Forum> GetMany() {
        // Load raw data from file:
        List<object>? rawData = FileManager.ReadFromJsonFileAsync(_filePath, new Forum()).Result;
        
        // If data is not null, continue, else abort by throwing an exception!
        if (rawData != null) {
            
            // Cast the loaded data to the proper load format:
            ForumList = rawData.OfType<Forum>().ToList();
            
            // Return the entire list:
            return ForumList.AsQueryable();
        } 
        throw new Exception(ErrorGetManyFailed);
    }
}