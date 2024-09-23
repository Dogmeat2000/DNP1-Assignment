using Entities;
using FileRepositories.Persistance;
using RepositoryContracts;

namespace FileRepositories.Repositories;

public class CommentFileRepository : ICommentRepository {

    private readonly string _filePath = Directory.GetCurrentDirectory() + @"\DataFiles\comments.json";
    private IFilePersistance FileManager { get; } = new FilePersistance();
    private List<Comment> CommentList { get; set; } = [];
    public string ErrorAddFailed { get; } = "Error occured while adding Comment. Data failed to load.";
    public string ErrorUpdateFailed { get; } = "Error occured while updating Comment. Data failed to load.";
    public string ErrorDeleteFailed { get; } = "Error occured while deleting Comment. Data failed to load.";
    public string ErrorGetSingleFailed { get; } = "Error occured while retrieving a single Comment. Data failed to load.";
    public string ErrorGetManyFailed { get; } = "Error occured while retrieving all Comments. Data failed to load.";


    public async Task<Comment> AddAsync(Comment comment) {
        
        // Load raw data from file:
        List<object>? rawData = await FileManager.ReadFromJsonFileAsync(_filePath, new Comment());
        
        // If data is not null, continue, else abort by throwing an exception!
        if (rawData != null) {
            
            // Cast the loaded data to the proper load format:
            CommentList = rawData.OfType<Comment>().ToList();
            
            // Select a unique id for this Comment instance:
            comment.Comment_id = CommentList.Any() 
                ? CommentList.Max(c => c.Comment_id) + 1 
                : 1;
            
            // Assign a time of creation:
            comment.Timestamp_created = DateTime.Now;
            
            // Add the new comment to the list of Comments:
            CommentList.Add(comment);
            
            // Cast the modified data back to the proper save format, and attempt to save:
            if (await FileManager.SaveToJsonFileAsync(_filePath, CommentList.Cast<object>().ToList())) {
                Console.WriteLine($": Added comment with ID '{comment.Comment_id}' in Post '{comment.ParentPost_id}' in Forum '{comment.ParentForum_id}'");
            } else {
                Console.WriteLine($": ERROR DID NOT SAVE comment with ID '{comment.Comment_id}' in Post '{comment.ParentPost_id}' in Forum '{comment.ParentForum_id}'");
            }

        } else {
            throw new Exception(ErrorAddFailed);
        }
        
        return comment;
    }

    
    public async Task UpdateAsync(Comment comment) {
        
        // Load raw data from file:
        List<object>? rawData = await FileManager.ReadFromJsonFileAsync(_filePath, new Comment());
        
        // If data is not null, continue, else abort by throwing an exception!
        if (rawData != null) {
            
            // Cast the loaded data to the proper load format:
            CommentList = rawData.OfType<Comment>().ToList();
            
            // Check if the comment to modify actually exists:
            Comment? existingComment = CommentList.SingleOrDefault(c => c.Comment_id == comment.Comment_id && c.ParentPost_id == comment.ParentPost_id && c.ParentForum_id == comment.ParentForum_id);
            if (existingComment is null) {
                throw new InvalidOperationException($"Comment with ID '{comment.Comment_id}' in Post '{comment.ParentPost_id}' in Forum '{comment.ParentForum_id}' not found");
            }
            
            // If it does exist, Remove it from the list of Comments:
            CommentList.Remove(existingComment);
            
            // Assign/Update the modified date:
            comment.Timestamp_modified = DateTime.Now;
            
            // Add the modified Comment to the list:
            CommentList.Add(comment);
            
            // Cast the modified data back to the proper save format, and attempt to save:
            if (await FileManager.SaveToJsonFileAsync(_filePath, CommentList.Cast<object>().ToList())) {
                Console.WriteLine($": Modified comment with ID '{comment.Comment_id}' in Post '{comment.ParentPost_id}' in Forum '{comment.ParentForum_id}'");
            } else {
                Console.WriteLine($": ERROR DID NOT Modify comment with ID '{comment.Comment_id}' in Post '{comment.ParentPost_id}' in Forum '{comment.ParentForum_id}'");
            }
        } else {
            throw new Exception(ErrorUpdateFailed);
        }
    }

    
    public async Task DeleteAsync(int commentId, int postId, int forumId) {
        
        // Load raw data from file:
        List<object>? rawData = await FileManager.ReadFromJsonFileAsync(_filePath, new Comment());
        
        // If data is not null, continue, else abort by throwing an exception!
        if (rawData != null) {
            
            // Cast the loaded data to the proper load format:
            CommentList = rawData.OfType<Comment>().ToList();
            
            // Check if the comment to remove actually exists:
            Comment? commentToRemove = CommentList.SingleOrDefault(c => c.Comment_id == commentId && c.ParentPost_id == postId && c.ParentForum_id == forumId);
            if (commentToRemove is null) {
                throw new InvalidOperationException($"Comment with ID '{commentId}' in Post '{postId}' in Forum '{forumId}' not found");
            }
            
            // If it does exist, Remove it from the list of Comments:
            CommentList.Remove(commentToRemove);
            
            // Cast the modified data back to the proper save format, and attempt to save:
            if (await FileManager.SaveToJsonFileAsync(_filePath, CommentList.Cast<object>().ToList())) {
                Console.WriteLine($": DELETED comment with ID '{commentToRemove.Comment_id}' in Post '{commentToRemove.ParentPost_id}' in Forum '{commentToRemove.ParentForum_id}'");
            } else {
                Console.WriteLine($": ERROR DID NOT Delete comment with ID '{commentToRemove.Comment_id}' in Post '{commentToRemove.ParentPost_id}' in Forum '{commentToRemove.ParentForum_id}'");
            }

        } else {
            throw new Exception(ErrorDeleteFailed);
        }
    }
    
    
    public async Task<Comment> GetSingleAsync(int commentId, int postId, int forumId) {
                
        // Load raw data from file:
        List<object>? rawData = await FileManager.ReadFromJsonFileAsync(_filePath, new Comment());
        
        // If data is not null, continue, else abort by throwing an exception!
        if (rawData != null) {
            
            // Cast the loaded data to the proper load format:
            CommentList = rawData.OfType<Comment>().ToList();
            
            // Check if the comment actually exists:
            Comment? commentToReturn = CommentList.SingleOrDefault(c => c.Comment_id == commentId && c.ParentPost_id == postId && c.ParentForum_id == forumId);
            if (commentToReturn is null) {
                throw new InvalidOperationException($"Comment with ID '{commentId}' in Post '{postId}' in Forum '{forumId}' not found");
            }
            
            // If it does exist, return it:
            return commentToReturn;
        } 
        throw new Exception(ErrorGetSingleFailed);
    }

    
    public IQueryable<Comment> GetMany() {
        // Load raw data from file:
        List<object>? rawData = FileManager.ReadFromJsonFileAsync(_filePath, new Comment()).Result;
        
        // If data is not null, continue, else abort by throwing an exception!
        if (rawData != null) {
            
            // Cast the loaded data to the proper load format:
            CommentList = rawData.OfType<Comment>().ToList();
            
            // Return the entire list:
            return CommentList.AsQueryable();
        } 
        throw new Exception(ErrorGetManyFailed);
    }
}